using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.OrderMapper;
using PreOrderBlindBox.Services.Utils;

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
        private readonly ICurrentUserService _currentUserService;
        public OrderService(
            IOrderRepository orderRepository, ICartService cartService,
            IUnitOfWork unitOfWork, IUserRepository userRepository,
            INotificationService notificationService, IOrderDetailService orderDetailService,
            ICurrentUserService currentUserService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _orderDetailService = orderDetailService;
            _currentUserService = currentUserService;

        }

        public async Task<Order> CreateOrder(RequestCreateOrder requestCreateOrder)
        {
            int customerId = _currentUserService.GetUserId();
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customer = await _userRepository.GetByIdAsync(customerId);
                var staff = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Staff", includes: x => x.Role)).FirstOrDefault();
                var notificationForCustomer = (new RequestCreateNotification()).NotificationForCustomer(customerId);
                var notificationForStaff = (new RequestCreateNotification()).NotificationForStaff(customer.FullName, staff.UserId);
                List<ResponseCart> priceForCarts = await _cartService.IdentifyPriceForCartItem(customerId);
                if(priceForCarts.ToList().Any(x=>x.Price < 0)) 
                {
                    throw new Exception($"The cart contains {priceForCarts[0].Quantity} item with an incorrect price");
                }
                requestCreateOrder.Amount = priceForCarts.Sum(x => x.Price);

                var orderEntity = requestCreateOrder.toOrderEntity(customerId);
                await _orderRepository.InsertAsync(orderEntity);
                await _notificationService.CreatNotification(notificationForStaff);
                await _notificationService.CreatNotification(notificationForCustomer);
                await _unitOfWork.SaveChanges();
                await _orderDetailService.CreateOrderDetail(priceForCarts,orderEntity.OrderId);
                await _cartService.UpdateStatusOfCartByCustomerID(customerId);
                await _unitOfWork.CommitTransactionAsync();
                return orderEntity;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;

            }
        }

        public async Task<Pagination<Order>> GetAllOrder(PaginationParameter page)
        {
            var orders = await _orderRepository.GetAll(pagination: page, includes: x=>x.OrderDetails);
            //var itemsOrderDetail = orders.Select(x => x.toOrderRespone(x.OrderDetails.Sum(y => y.Quantity))).ToList();
            var result = new Pagination<Order>(orders, orders.Count, page.PageIndex, page.PageSize);
            return result;
        }

        /*public async Task<Pagination<Order>> GetAllOrder(PaginationParameter? page)
{
   var listOrderDb = await _orderRepository.GetAll(pagination: page, includes: x=> x.Customer);

   return; 

}*/

        public async Task<Order> GetOrderById(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
    }
}
