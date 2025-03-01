using AutoMapper;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using PreOrderBlindBox.Services.IServices;
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
                transactionCreate.CreatedDate = DateTime.Now;
                transactionCreate.OrderId = model.OrderId;
                transactionCreate.Status = TransactionStatusEnum.Success.ToString();

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

        public async Task<Pagination<ResponseTransactionResult>> GetListOfAllTransaction(PaginationParameter paginationParameter)
        {

            List<Transaction> transactions = await _transactionRepository.GetAll(pagination: paginationParameter);
            var response = _mapper.Map<List<ResponseTransactionResult>>(transactions);
            int totalItemsCount = _transactionRepository.Count();
            var responseMap = new Pagination<ResponseTransactionResult>(response, totalItemsCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return responseMap;
        }

        public async Task<Pagination<ResponseTransactionResult>> GetListOfTransactionByUser(PaginationParameter paginationParameter, int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            if(user.WalletId == null)
            {
                return null;
            }
            List<Transaction> transactions = await _transactionRepository.GetAll(pagination: paginationParameter, filter:x=>x.WalletId== user.WalletId);
            var response = _mapper.Map<List<ResponseTransactionResult>>(transactions);
            int totalItemsCount = _transactionRepository.Count();
            var responseMap = new Pagination<ResponseTransactionResult>(response, totalItemsCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return responseMap;
        }
    }

}
