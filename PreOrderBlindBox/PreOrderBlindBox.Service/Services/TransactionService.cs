using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using PreOrderBlindBox.Services.IServices;
using System.ComponentModel;

namespace PreOrderBlindBox.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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
                
                if (transactionDetail.WalletId!= user.WalletId)
                {
                    return null;
                }
                var detailTransaction = _mapper.Map<ResponseTransactionResult>(transactionDetail);
                return detailTransaction;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
