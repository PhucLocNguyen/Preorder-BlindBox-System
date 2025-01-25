using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.DTO.ResponeDTO.WalletModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers;

namespace PreOrderBlindBox.Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
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
            userDetail.WalletId = wallet.WalletId;
            await _userRepository.UpdateAsync(userDetail);
            return true;
        }

        public async Task<bool> DepositAsync(int userId, decimal amount)
        {
            // validate payment cua momo truoc roi moi add payment

            
            // Them tien vao wallet
            var userDetail = await _userRepository.GetByIdAsync(userId);
            if (userDetail == null)
            {
                return false;
            }
            if (userDetail.WalletId == null)
            {
                return false;
            }
            Wallet wallet = await _walletRepository.GetByIdAsync(userDetail.WalletId);
            decimal walletBalance = wallet.Balance;
            wallet.Balance += amount;
            await _walletRepository.UpdateAsync(wallet);
            Transaction transaction = new Transaction()
            {
                Money = amount,
                CreatedDate = DateTime.Now,
                Type = "Deposit",
                WalletId = wallet.WalletId,
                BalanceAtTime = walletBalance
            };
            await _transactionRepository.InsertAsync(transaction);
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
            return wallet == null ? null : _mapper.Map<ResponseShowWallet>(wallet);
        }

        public Task<bool> WithdrawAsync(int userId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
