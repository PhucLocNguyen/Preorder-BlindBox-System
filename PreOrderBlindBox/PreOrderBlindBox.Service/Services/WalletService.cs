using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
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
        private readonly IPaymentSerivce _paymentSerivce;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository, ITransactionRepository transactionRepository, IMapper mapper, IUnitOfWork unitOfWork, IPaymentSerivce paymentService)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _paymentSerivce = paymentService;
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
            return wallet == null ? null : _mapper.Map<ResponseShowWallet>(wallet);
        }

        public async Task<bool> WithdrawAsync(int userId, decimal amount)
        {
            try
            {
                var wallet = await _walletRepository.GetByIdAsync(userId);
                if( amount < 0  )
                {
                    throw new Exception("The amount must be larger than 0");
                }
                wallet.Balance = amount;
                await _walletRepository.UpdateAsync(wallet);
                await _unitOfWork.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
