using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestDTO;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.CartMapper;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace PreOrderBlindBox.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateCart(RequestCreateCart requestCreateCart)
        {
            try
            {
                var CartEntity = requestCreateCart.toCartEntity();
                await _cartRepository.InsertAsync(CartEntity);
                await _unitOfWork.SaveChanges();

            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Cart>> GetAllCartByCustomerID(int customerID)
        {
            return await _cartRepository.GetAll(filter: x=> (x.UserId == customerID) && (x.IsDeleted == false));
        }

        public async Task UpdateStatusOfCartByCustomerID(int customerID)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var listCartByCustomerID = await GetAllCartByCustomerID(customerID);
                foreach (var item in listCartByCustomerID)
                {
                    item.IsDeleted = true;
                    await _cartRepository.UpdateAsync(item);
                }
                await _unitOfWork.SaveChanges();
                await _unitOfWork.CommitTransactionAsync();

            } catch (Exception ex)
            {
                
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Something went wrong when updating with cart", ex);
            }
            
        }
    }
}
