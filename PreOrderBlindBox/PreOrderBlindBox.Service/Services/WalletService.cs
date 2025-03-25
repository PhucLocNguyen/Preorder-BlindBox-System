using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.WalletModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.WalletModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers;
using PreOrderBlindBox.Services.Mappers.WalletMapper;

namespace PreOrderBlindBox.Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository, ITransactionRepository transactionRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateWalletAsync(int userId)
        {
            User userDetail = await _userRepository.GetByIdAsync(userId);

            if (userDetail == null || userDetail.WalletId != null)
            {
                return false;
            }

            Wallet wallet = new Wallet()
            {
                Balance = 0,
                CreatedDate = DateTime.Now
            };

            await _walletRepository.InsertAsync(wallet);
            await _unitOfWork.SaveChanges();
            userDetail.WalletId = wallet.WalletId;
            await _userRepository.UpdateAsync(userDetail);
            return true;
        }


        public async Task<ResponseShowWallet> GetWalletByUserIdAsync(int userId)
        {
            var userDetail = await _userRepository.GetByIdAsync(userId);
            if (userDetail == null)
            {
                return null;
            }
            if (userDetail.WalletId == null)
            {
                return null;
            }
            Wallet wallet = await _walletRepository.GetByIdAsync(userDetail.WalletId);
            return wallet == null ? null : wallet.toResponseShowWallet();
        }

        public async Task<bool> WithdrawAsync(int userId, decimal amount)
        {
            try
            {
                var wallet = await _walletRepository.GetByIdAsync(userId);
                if (amount < 0)
                {
                    throw new Exception("The amount must be larger than 0");
                }
                wallet.Balance = amount;
                await _transactionRepository.InsertAsync(new Transaction()
                {
                    Money = amount,
                    CreatedDate = DateTime.Now,
                    WalletId = wallet.WalletId,
                    Type = TypeOfTransactionEnum.Withdraw.ToString(),
                    Description = "Withdraw money from wallet",
                    BalanceAtTime = wallet.Balance,
                    Status = TransactionStatusEnum.Success.ToString()
                });
                await _walletRepository.UpdateAsync(wallet);
                await _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHistoryWallet> ShowDetailWalletAtTime(int userId, RequestShowHistoryWallet model)
        {
            var userDetail = await _userRepository.GetByIdAsync(userId);
            if (userDetail == null || userDetail.WalletId == null)
            {
                return null;
            }

            Wallet wallet = await _walletRepository.GetByIdAsync(userDetail.WalletId);
            if (wallet == null)
            {
                return null;
            }

            var startDate = new DateTime(model.Year, model.Month, 1);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);

            var listTransactionAtTime = await _transactionRepository.GetAllFullIncludeTransaction(filter: x => x.WalletId==userDetail.WalletId && (x.CreatedDate >= startDate && x.CreatedDate <= endDate),orderBy:x=>x.OrderByDescending(x =>x.TransactionId));
            decimal closingBalance;
            if (listTransactionAtTime.Count == 0)
            {
                var previousTransaction = await _transactionRepository.GetAllFullIncludeTransaction(filter:x=> x.WalletId == userDetail.WalletId && x.Status.Equals(TransactionStatusEnum.Success.ToString()) && (x.CreatedDate<=startDate),orderBy:x=>x.OrderByDescending(x=>x.TransactionId));
                if (previousTransaction.Any())
                {
                    decimal previousBalance = previousTransaction.First().BalanceAtTime ?? 0;
                    if (previousTransaction.First().Type.Equals(TypeOfTransactionEnum.Recharge.ToString()) || previousTransaction.First().Type.Equals(TypeOfTransactionEnum.Refund.ToString()))
                    {
                        closingBalance = previousBalance + previousTransaction.First().Money;
                    }
                    else
                    {
                        closingBalance = previousBalance - previousTransaction.First().Money;
                    }
                }
                else { 
                
                closingBalance = 0;
                }
            }
            else
            {
                if (listTransactionAtTime.First().Type.Equals(TypeOfTransactionEnum.Recharge.ToString())|| listTransactionAtTime.First().Type.Equals(TypeOfTransactionEnum.Refund.ToString()))
                {
                    closingBalance = listTransactionAtTime.First().BalanceAtTime.Value + listTransactionAtTime.First().Money;
                }
                else
                {
                    closingBalance = listTransactionAtTime.First().BalanceAtTime.Value - listTransactionAtTime.First().Money;
                }
            }
            var listTransactionAtTimeWithStatus = await _transactionRepository.GetAllFullIncludeTransaction(filter: x => x.WalletId == userDetail.WalletId && (x.CreatedDate >= startDate && x.CreatedDate <= endDate), orderBy: x => x.OrderByDescending(x => x.TransactionId));

            var totalWithdrawPending = listTransactionAtTimeWithStatus.Where(t => t.Type.Equals(TypeOfTransactionEnum.Withdraw.ToString()) && t.Status.Equals(TransactionStatusEnum.Pending.ToString())).Sum(t => t.Money);
            var totalPurchase = listTransactionAtTimeWithStatus.Where(t => t.Type.Equals(TypeOfTransactionEnum.Purchase.ToString()) && t.Status.Equals(TransactionStatusEnum.Success.ToString())).Sum(t => t.Money);
            var totalRefund = listTransactionAtTimeWithStatus.Where(t => t.Type.Equals(TypeOfTransactionEnum.Refund.ToString()) && t.Status.Equals(TransactionStatusEnum.Success.ToString())).Sum(t => t.Money);
            var totalDeposited = listTransactionAtTimeWithStatus.Where(t =>t.Type.Equals(TypeOfTransactionEnum.Recharge.ToString())&& t.Status.Equals(TransactionStatusEnum.Success.ToString())).Sum(t => t.Money);
            var totalWithdrawn = listTransactionAtTimeWithStatus.Where(t => t.Type.Equals(TypeOfTransactionEnum.Withdraw.ToString()) && t.Status.Equals(TransactionStatusEnum.Success.ToString())).Sum(t => t.Money);
            return await Task.FromResult(new ResponseHistoryWallet()
            {
                BalanceAtTime = closingBalance,
                TotalDepositAtTime = totalDeposited,
                TotalWithdrawAtTime = totalWithdrawn,
                TotalPayAtTime = totalPurchase,
                TotalRefundAtTime = totalRefund,
                TotalWithdrawPendingAtTime = totalWithdrawPending
            });
        }

    }
}
