using AutoMapper;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.TransactionMapper;
using System.ComponentModel;

namespace PreOrderBlindBox.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, IMapper mapper, IWalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _walletRepository = walletRepository;
        }



        public async Task<bool> CreateTransaction(RequestTransactionCreateModel model)
        {
            var transactionCreate = _mapper.Map<Transaction>(model);
            Wallet wallet = null;
            try
            {
                if (transactionCreate == null)
                {
                    throw new Exception("Transaction information can't be null");
                }
                if (transactionCreate.WalletId == 0)
                {
                    throw new Exception("Invalid wallet !");
                }

                wallet = await _walletRepository.GetByIdAsync(transactionCreate.WalletId.Value);
                if (wallet == null)
                {
                    throw new Exception("Invalid wallet !");
                }

                transactionCreate.BalanceAtTime = wallet.Balance;

                if (model.Type == TypeOfTransactionEnum.Refund || model.Type == TypeOfTransactionEnum.Recharge)
                {
                    wallet.Balance += model.Money;

                }
                else if (model.Type == TypeOfTransactionEnum.Purchase || model.Type == TypeOfTransactionEnum.Withdraw)
                {
                    if (wallet.Balance < model.Money)
                    {
                        throw new Exception("Not enough money in your wallet !");
                    }
                    wallet.Balance -= model.Money;
                }
                await _walletRepository.UpdateAsync(wallet);
                transactionCreate.Description = model.Description;
                transactionCreate.CreatedDate = DateTime.UtcNow.AddHours(7);
                transactionCreate.OrderId = model.OrderId;
                transactionCreate.TempCampaignBulkOrderId = model.TempCampaignBulkOrderId;
                transactionCreate.Status = model.Status.ToString();

                _transactionRepository.AddTransaction(transactionCreate);
                return true;
            }
            catch (Exception ex)
            {
                transactionCreate.CreatedDate = DateTime.Now;
                transactionCreate.Description = $"Failed: {ex.Message}";
                transactionCreate.Status = TransactionStatusEnum.Failed.ToString();
                _transactionRepository.AddTransaction(transactionCreate);
                return false;
            }
        }

        public async Task<bool> CreateWithdrawRequest(int userId, decimal money)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            try
            {
                if(string.IsNullOrEmpty(user.BankName) || string.IsNullOrEmpty(user.BankAccountNumber))
                {
                    return false;
                }
                var walletDetail = await _walletRepository.GetByIdAsync(user.WalletId.Value);
                if (walletDetail == null)
                {
                    return false;
                }
                if (walletDetail.Balance < money)
                {
                    return false;
                }
                var requestCustomerTransactionCreateModel = new RequestTransactionCreateModel()
                {
                    Money = money,
                    WalletId = user.WalletId,
                    Description = "Withdraw money from website wallet",
                    Type = TypeOfTransactionEnum.Withdraw,
                    Status = TransactionStatusEnum.Pending,
                    BalanceAtTime = walletDetail.Balance,
                    
                };
                return await CreateTransaction(requestCustomerTransactionCreateModel);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> ConfirmWithdrawTransaction(int transactionId)
        {
            var transaction = await _transactionRepository.GetDetailTransaction(transactionId);
            if (transaction == null)
            {
                return false;
            }
            if (transaction.Status.Equals(TransactionStatusEnum.Pending.ToString()) && transaction.Type.Equals(TypeOfTransactionEnum.Withdraw.ToString()))
            {
                transaction.Status = TransactionStatusEnum.Success.ToString();
                await _transactionRepository.UpdateAsync(transaction);

                var admin = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Admin" && x.WalletId!=null, includes: [x => x.Role, x => x.Wallet])).FirstOrDefault();
                Wallet systemWallet = await _walletRepository.GetByIdAsync(admin.WalletId.Value);
                var systemTransaction = new RequestTransactionCreateModel()
                {
                    Money = transaction.Money,
                    WalletId = systemWallet.WalletId,
                    Description = "Withdraw money from system wallet to pay for customer",
                    Type = TypeOfTransactionEnum.Withdraw,
                    BalanceAtTime = systemWallet.Balance,
                    Status = TransactionStatusEnum.Success
                };
                await CreateTransaction(systemTransaction);
                return true;
            }
            return false;
        }

        public async Task<ResponseTransactionResult> GetDetailTransactionVerifyUser(string transactionId, int userId)
        {
            try
            {
                var user = _userRepository.GetById(userId);
                if (user == null)
                {
                    return null;
                }
                Transaction transactionDetail = await _transactionRepository.GetDetailTransaction(Int32.Parse(transactionId));
                if (transactionDetail == null)
                {
                    return null;
                }

                if (transactionDetail.WalletId != user.WalletId)
                {
                    return null;
                }
                var detailTransaction = _mapper.Map<ResponseTransactionResult>(transactionDetail);
                return detailTransaction;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Pagination<ResponseTransactionResult>> GetListOfAllTransaction(RequestTransactionReportModel model)
        {
            List<Transaction> transactions = new();
            int totalCount = 0;
            (totalCount, transactions) = await _transactionRepository.GetListOfAllTransaction(paginationParameters: model.PaginationParameter, model.Type, model.FromDate, model.EndDate, orderBy: x => x.OrderBy(x => x.CreatedDate));
            var response = _mapper.Map<List<ResponseTransactionResult>>(transactions);
            var responseMap = new Pagination<ResponseTransactionResult>(response, totalCount, model.PaginationParameter.PageIndex, model.PaginationParameter.PageSize);
            return responseMap;
        }

        public async Task<Pagination<ResponseTransactionResult>> GetListOfTransactionByUser(PaginationParameter paginationParameter, int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            if (user.WalletId == null)
            {
                return null;
            }
            List<Transaction> transactions = await _transactionRepository.GetAll(pagination: paginationParameter, filter: x => x.WalletId == user.WalletId);
            var response = _mapper.Map<List<ResponseTransactionResult>>(transactions);
            int totalItemsCount = _transactionRepository.Count(filter: x => x.WalletId == user.WalletId);
            var responseMap = new Pagination<ResponseTransactionResult>(response, totalItemsCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return responseMap;
        }
        public async Task<Pagination<ResponsePendingWithdraw>> GetListPendingWithdrawRequest(PaginationParameter paginationParameter)
        {
            List<Transaction> transactions = await _transactionRepository.GetAllFullIncludeTransaction(pagination: paginationParameter, filter: x => x.Status == TransactionStatusEnum.Pending.ToString() && x.Type == TypeOfTransactionEnum.Withdraw.ToString());
            var response = transactions.Select(x => x.toReposonePendingWithdrawTransaction()).ToList();
            int totalItemsCount = _transactionRepository.Count(filter: x => x.Status == TransactionStatusEnum.Pending.ToString() && x.Type == TypeOfTransactionEnum.Withdraw.ToString());
            var responseMap = new Pagination<ResponsePendingWithdraw>(response, totalItemsCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return responseMap;
        }
        public async Task<ResponsePendingWithdraw> GetDetailPendingWithdraw(int transactionId)
        {
            Transaction transactionDetail = await _transactionRepository.GetDetailTransaction(transactionId);
            var response = transactionDetail.toReposonePendingWithdrawTransaction();
            return response;
        }
    }

}
