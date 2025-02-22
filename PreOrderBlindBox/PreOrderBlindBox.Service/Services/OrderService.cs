using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.OrderDetailMapper;
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
        private readonly IUserVoucherService _userVoucherService;

        public OrderService(
            IOrderRepository orderRepository, ICartService cartService,
            IUnitOfWork unitOfWork, IUserRepository userRepository,
            INotificationService notificationService, IOrderDetailService orderDetailService,
            ICurrentUserService currentUserService,
            IUserVoucherService userVoucherService
            )
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _orderDetailService = orderDetailService;
            _currentUserService = currentUserService;
            _userVoucherService = userVoucherService;

        }

        public async Task<Order> CreateOrder(RequestCreateOrder requestCreateOrder, RequestCreateCart? requestCreateCart)
        {
            int customerId = _currentUserService.GetUserId();
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customer = await _userRepository.GetByIdAsync(customerId);
                var staff = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Staff", includes: x => x.Role)).FirstOrDefault();
                var notificationForCustomer = (new RequestCreateNotification()).NotificationForCustomer(customerId);
                var notificationForStaff = (new RequestCreateNotification()).NotificationForStaff(customer.FullName, staff.UserId);
                List<ResponseCart> priceForCarts = await _cartService.IdentifyPriceForCartItem(customerId, requestCreateCart);
                if(priceForCarts.ToList().Any(x=>x.Price < 0)) 
                {
                    throw new Exception($"The cart contains {priceForCarts[0].Quantity} item with an incorrect price");
                }
                if(requestCreateOrder.UserVoucherId != null)
                {
                    var userVoucher = await _userVoucherService.GetUserVoucherById((int)requestCreateOrder.UserVoucherId);
                    var amountAfterUsingVoucher = priceForCarts.Sum(x => x.Amount) * (userVoucher.PercentDiscount/100);
                    if(amountAfterUsingVoucher > userVoucher.MaximumMoneyDiscount)
                    {
						requestCreateOrder.Amount = userVoucher.MaximumMoneyDiscount;
                    }
                    else
                    {
						requestCreateOrder.Amount = amountAfterUsingVoucher;
					}
				}else
                {
					requestCreateOrder.Amount = priceForCarts.Sum(x => x.Amount);
				}

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

        public async Task<Pagination<ResponseOrder>> GetAllOrder(PaginationParameter page, string? searchKeyWords)
        {
            var orders = await _orderRepository.GetAll(filter: x=> (x.ReceiverName.Contains(searchKeyWords) || x.ReceiverAddress.Contains(searchKeyWords) || String.IsNullOrEmpty(searchKeyWords))
                                                        ,pagination: page, includes: x=>x.OrderDetails);
            var itemsOrderDetail = orders.Select(x => x.toOrderRespone()).ToList();
            var result = new Pagination<ResponseOrder>(itemsOrderDetail, itemsOrderDetail.Count, page.PageIndex, page.PageSize);
            return result;
        }

        /*public async Task<Pagination<Order>> GetAllOrder(PaginationParameter? page)
{
   var listOrderDb = await _orderRepository.GetAll(pagination: page, includes: x=> x.Customer);

   return; 

}*/

        public async Task<ResponseOrder> GetOrderById(int id)
        {
            var orderById = await _orderRepository.GetAll(filter: x => x.OrderId == id, includes: x => x.OrderDetails);
            var orderByIdResponse = orderById.FirstOrDefault().toOrderRespone();

			return orderByIdResponse;
        }
    }
}
