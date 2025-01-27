using Azure;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponeModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.OrderMapper;

namespace PreOrderBlindBox.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUserRepository _userRepository;
        private readonly ICartService _cartService;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(
            IOrderRepository orderRepository, ICartService cartService,
            IUnitOfWork unitOfWork, IUserRepository userRepository,
            INotificationService notificationService, IOrderDetailService orderDetailService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _orderDetailService = orderDetailService;
        }

        public async Task<Order> CreateOrder(RequestCreateOrder requestCreateOrder)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customer = await _userRepository.GetByIdAsync(requestCreateOrder.CustomerId);
                var staff = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Staff", includes: x => x.Role)).FirstOrDefault();
                var notificationForCustomer = (new RequestCreateNotification()).NotificationForCustomer(requestCreateOrder.CustomerId);
                var notificationForStaff = (new RequestCreateNotification()).NotificationForStaff(customer.FullName, staff.UserId);
                List<ResponeCart> priceForCarts = await _cartService.IdentifyPriceForCartItem(requestCreateOrder.CustomerId);
                requestCreateOrder.Amount = priceForCarts.Sum(x => x.Price);

                var orderEntity = requestCreateOrder.toOrderEntity();
                await _orderRepository.InsertAsync(orderEntity);
                await _notificationService.CreatNotification(notificationForStaff);
                await _notificationService.CreatNotification(notificationForCustomer);
                await _unitOfWork.SaveChanges();
                await _orderDetailService.CreateOrderDetail(priceForCarts,orderEntity.OrderId);
                await _cartService.UpdateStatusOfCartByCustomerID(requestCreateOrder.CustomerId);
                await _unitOfWork.CommitTransactionAsync();
                return orderEntity;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Something went wrong when order create", ex);

            }
        }

        public async Task<Pagination<Order>> GetAllOrder(PaginationParameter page)
        {
            var Orders = await _orderRepository.GetAll(pagination: page);
            var result = new Pagination<Order>(Orders,Orders.Count, page.PageIndex, page.PageSize);
            return result;
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
