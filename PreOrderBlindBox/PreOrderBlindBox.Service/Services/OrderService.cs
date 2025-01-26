using Azure;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestDTO;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestDTO;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartService _cartService;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(
            IOrderRepository orderRepository, ICartService cartService, 
            IUnitOfWork unitOfWork, IUserRepository userRepository,
            INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _notificationService = notificationService;
            
        }

        public async Task<int> CreateOrder(RequestCreateOrder requestCreateOrder)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Customer = await _userRepository.GetByIdAsync(requestCreateOrder.CustomerId);
                var Staff = (await _userRepository.GetAll(filter: x=>x.Role.RoleName == "Staff" , includes: x=> x.Role)).FirstOrDefault();
                var listOrderDetails = await _cartService.GetAllCartByCustomerID(requestCreateOrder.CustomerId);
                var notificationForCustomer = (new RequestCreateNotification()).NotificationForCustomer(requestCreateOrder.CustomerId);
                var notificationForStaff = (new RequestCreateNotification()).NotificationForStaff(Customer.FullName, Staff.UserId);

                await _notificationService.CreatNotification(notificationForStaff);
                await _notificationService.CreatNotification(notificationForCustomer);
                await _unitOfWork.SaveChanges();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
            
            throw new NotImplementedException();

        }

        public Task<Pagination<Order>> GetAllOrder(PaginationParameter page)
        {
            throw new NotImplementedException();
        }

        /*public async Task<Pagination<Order>> GetAllOrder(PaginationParameter? page)
{
   var listOrderDb = await _orderRepository.GetAll(pagination: page, includes: x=> x.Customer);

   return; 

}*/

        public Task<Order> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
