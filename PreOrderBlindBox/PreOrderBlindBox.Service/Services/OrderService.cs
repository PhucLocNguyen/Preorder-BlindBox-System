using Azure;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Service.Services;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.CartMapper;
using PreOrderBlindBox.Services.Mappers.OrderDetailMapper;
using PreOrderBlindBox.Services.Mappers.OrderMapper;
using PreOrderBlindBox.Services.Mappers.TempCampaignBulkOrderMapper;
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
        private readonly ITransactionService _transactionService;
        private readonly ITempCampaignBulkOrderRepository _tempCampaignBulkOrderRepository;
        private readonly ITempCampaignBulkOrderDetailService _tempCampaignBulkOrderDetailService;
        private readonly IPreorderCampaignRepository _preorderCampaignRepository;

        public OrderService(
            IOrderRepository orderRepository, ICartService cartService,
            IUnitOfWork unitOfWork, IUserRepository userRepository,
            INotificationService notificationService, IOrderDetailService orderDetailService,
            ICurrentUserService currentUserService,
            IUserVoucherService userVoucherService,
            ITransactionService transactionService,
            ITempCampaignBulkOrderRepository tempCampaignBulkOrderRepository,
            ITempCampaignBulkOrderDetailService tempCampaignBulkOrderDetailService,
        IPreorderCampaignRepository preorderCampaignRepository
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
            _transactionService = transactionService;
            _tempCampaignBulkOrderRepository = tempCampaignBulkOrderRepository;
            _preorderCampaignRepository = preorderCampaignRepository;
            _tempCampaignBulkOrderDetailService = tempCampaignBulkOrderDetailService;
        }

        public async Task CreateOrder(RequestCreateOrder requestCreateOrder)
        {
            int customerId = _currentUserService.GetUserId();
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customer = await _userRepository.GetByIdAsync(customerId);
                var staff = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Staff", includes: x => x.Role)).FirstOrDefault();
                var admin = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Admin", includes: x => x.Role)).FirstOrDefault();

                var notificationForCustomer = (new RequestCreateNotification()).NotificationForCustomer(customerId);
                var notificationForStaff = (new RequestCreateNotification()).NotificationForStaff(customer.FullName, staff.UserId);

                var requestAdminTransactionCreateModel = new RequestTransactionCreateModel()
                {
                    Description = "Recharge",
                    BalanceAtTime = 0,
                    Money = 0,
                    WalletId = admin.UserId,
                    Type = TypeOfTransactionEnum.Recharge,
                };

                var requestCustomerTransactionCreateModel = new RequestTransactionCreateModel()
                {
                    Description = "Purchase",
                    BalanceAtTime = 0,
                    Money = 0,
                    WalletId = customerId,
                    Type = TypeOfTransactionEnum.Purchase,
                };

                /*var customerWallet = await _walletService.GetWalletByUserIdAsync(customerId); 
				var adminWallet = await _walletService.GetWalletByUserIdAsync(admin.UserId);*/

                List<ResponseCart> priceForCarts = await _cartService.IdentifyPriceForCartItem(customerId, requestCreateOrder.RequestCreateCart);
                decimal totalAmountInOrder = 0;
                if (priceForCarts.Count == 0)
                    throw new Exception("The cart is empty");
                if (priceForCarts.ToList().Any(x => x.Price < 0))
                {
                    throw new Exception($"The cart contains {priceForCarts[0].Quantity} item with an incorrect price");
                }

                int preorderCampaignId = (int)priceForCarts[0].PreorderCampaignId;

                if (priceForCarts.Count == 1)
                {
                    var preorderCampaign = await _preorderCampaignRepository.GetDetailPreorderCampaignById(preorderCampaignId);
                    decimal amountAfterVoucher = 0;
                    int? voucherID = requestCreateOrder.UserVoucherIdForPreorderCampaign.ContainsKey(preorderCampaignId) ? requestCreateOrder.UserVoucherIdForPreorderCampaign[preorderCampaignId] : null;
                    if (requestCreateOrder.UserVoucherIdForPreorderCampaign.ContainsKey(preorderCampaignId))
                    {

                        //Chỗ này cần sửa lại 
                        var userVoucher = await _userVoucherService.GetUserVoucherById(requestCreateOrder.UserVoucherIdForPreorderCampaign[preorderCampaignId]);
                        if (userVoucher.Quantity > userVoucher.UsedQuantity)
                            throw new Exception("You have used up all of these vouchers.");
                        var amountUsingVoucher = priceForCarts.Sum(x => x.Amount) * (userVoucher.PercentDiscount / 100);
                        if (amountUsingVoucher > userVoucher.MaximumMoneyDiscount)
                        {
                            amountAfterVoucher = (decimal)userVoucher.MaximumMoneyDiscount;
                        }
                        else
                        {
                            amountAfterVoucher = (decimal)amountUsingVoucher;

                        }
                    }
                    requestCreateOrder.Amount = priceForCarts.Sum(x => x.Amount) - amountAfterVoucher;

                    if (preorderCampaign.Type.Equals("BulkOrder"))
                    {
                        var tempCampaignBulkOrderEntity = requestCreateOrder.toTempCampaignBulkOrder(customerId, voucherID);
                        await _tempCampaignBulkOrderRepository.InsertAsync(tempCampaignBulkOrderEntity);
                        await _unitOfWork.SaveChanges();
                        await _tempCampaignBulkOrderDetailService.CreateTempCampaignBulkOrderDetail(priceForCarts, tempCampaignBulkOrderEntity.TempCampaignBulkOrderId);
                    }
                    else
                    {
                        var orderEntity = requestCreateOrder.toOrderEntity(customerId, voucherID);
                        await _orderRepository.InsertAsync(orderEntity);
                        await _unitOfWork.SaveChanges();
                        await _orderDetailService.CreateOrderDetail(priceForCarts, orderEntity.OrderId);
                    }
                    totalAmountInOrder += (decimal)requestCreateOrder.Amount;
                    if (voucherID != null)
                        _userVoucherService.UpdateUserVoucherAsync(new DTO.RequestDTO.UserVoucherModel.RequestUpdateUserVoucher() { VoucherCampaignId = (int)voucherID });
                }
                List<ResponseCart> cartInSameCampaign = new List<ResponseCart>();
                cartInSameCampaign.Add(priceForCarts[0]);
                for (int i = 1; i < priceForCarts.Count; i++)
                {
                    if (priceForCarts[i].PreorderCampaignId != preorderCampaignId)
                    {
                        var preorderCampaign = await _preorderCampaignRepository.GetDetailPreorderCampaignById(preorderCampaignId);
                        decimal amountAfterVoucher = 0;

                        //Chỗ này cần sửa lại 
                        int? voucherID = requestCreateOrder.UserVoucherIdForPreorderCampaign.ContainsKey(preorderCampaignId) ? requestCreateOrder.UserVoucherIdForPreorderCampaign[preorderCampaignId] : null;
                        if (requestCreateOrder.UserVoucherIdForPreorderCampaign.ContainsKey(preorderCampaignId))
                        {
                            var userVoucher = await _userVoucherService.GetUserVoucherById(requestCreateOrder.UserVoucherIdForPreorderCampaign[preorderCampaignId]);
                            if (userVoucher.Quantity > userVoucher.UsedQuantity)
                                throw new Exception("You have used up all of these vouchers.");
                            var amountUsingVoucher = cartInSameCampaign.Sum(x => x.Amount) * (userVoucher.PercentDiscount / 100);
                            if (amountUsingVoucher > userVoucher.MaximumMoneyDiscount)
                            {
                                amountAfterVoucher = (decimal)userVoucher.MaximumMoneyDiscount;
                            }
                            else
                            {
                                amountAfterVoucher = (decimal)amountUsingVoucher;

                            }
                        }
                        requestCreateOrder.Amount = cartInSameCampaign.Sum(x => x.Amount) - amountAfterVoucher;

                        if (preorderCampaign.Type.Equals("BulkOrder"))
                        {
                            var tempCampaignBulkOrderEntity = requestCreateOrder.toTempCampaignBulkOrder(customerId, voucherID);
                            await _tempCampaignBulkOrderRepository.InsertAsync(tempCampaignBulkOrderEntity);
                            await _unitOfWork.SaveChanges();
                            await _tempCampaignBulkOrderDetailService.CreateTempCampaignBulkOrderDetail(cartInSameCampaign, tempCampaignBulkOrderEntity.TempCampaignBulkOrderId);
                        }
                        else
                        {
                            var orderEntity = requestCreateOrder.toOrderEntity(customerId, voucherID);
                            await _orderRepository.InsertAsync(orderEntity);
                            await _unitOfWork.SaveChanges();
                            await _orderDetailService.CreateOrderDetail(cartInSameCampaign, orderEntity.OrderId);
                        }
                        totalAmountInOrder += (decimal)requestCreateOrder.Amount;
                        if (voucherID != null)
                            _userVoucherService.UpdateUserVoucherAsync(new DTO.RequestDTO.UserVoucherModel.RequestUpdateUserVoucher() { VoucherCampaignId = (int)voucherID });
                        preorderCampaign.PlacedOrderCount += cartInSameCampaign.Sum(x => x.Quantity);
                        cartInSameCampaign = new List<ResponseCart> { priceForCarts[i] };
                        preorderCampaignId = (int)priceForCarts[i].PreorderCampaignId;

                    }
                    else if (priceForCarts[i].PreorderCampaignId == preorderCampaignId)
                    {
                        cartInSameCampaign.Add(priceForCarts[i]);
                    }
                    if (i == priceForCarts.Count - 1)
                    {
                        var preorderCampaign = await _preorderCampaignRepository.GetDetailPreorderCampaignById(preorderCampaignId);
                        decimal amountAfterVoucher = 0;
                        int? voucherID = requestCreateOrder.UserVoucherIdForPreorderCampaign.ContainsKey(preorderCampaignId) ? requestCreateOrder.UserVoucherIdForPreorderCampaign[preorderCampaignId] : null;
                        if (requestCreateOrder.UserVoucherIdForPreorderCampaign.ContainsKey(preorderCampaignId))
                        {

                            //Chỗ này cần sửa lại 
                            var userVoucher = await _userVoucherService.GetUserVoucherById(requestCreateOrder.UserVoucherIdForPreorderCampaign[preorderCampaignId]);
                            if (userVoucher.Quantity > userVoucher.UsedQuantity)
                                throw new Exception("You have used up all of these vouchers.");
                            var amountUsingVoucher = cartInSameCampaign.Sum(x => x.Amount) * (userVoucher.PercentDiscount / 100);
                            if (amountUsingVoucher > userVoucher.MaximumMoneyDiscount)
                            {
                                amountAfterVoucher = (decimal)userVoucher.MaximumMoneyDiscount;
                            }
                            else
                            {
                                amountAfterVoucher = (decimal)amountUsingVoucher;

                            }
                        }
                        requestCreateOrder.Amount = cartInSameCampaign.Sum(x => x.Amount) - amountAfterVoucher;
                        if (preorderCampaign.Type.Equals("BulkOrder"))
                        {
                            var tempCampaignBulkOrderEntity = requestCreateOrder.toTempCampaignBulkOrder(customerId, voucherID);
                            await _tempCampaignBulkOrderRepository.InsertAsync(tempCampaignBulkOrderEntity);
                            await _unitOfWork.SaveChanges();
                            await _tempCampaignBulkOrderDetailService.CreateTempCampaignBulkOrderDetail(cartInSameCampaign, tempCampaignBulkOrderEntity.TempCampaignBulkOrderId);
                        }
                        else
                        {
                            var orderEntity = requestCreateOrder.toOrderEntity(customerId, voucherID);
                            await _orderRepository.InsertAsync(orderEntity);
                            await _unitOfWork.SaveChanges();
                            await _orderDetailService.CreateOrderDetail(cartInSameCampaign, orderEntity.OrderId);
                        }
                        totalAmountInOrder += (decimal)requestCreateOrder.Amount;
                        if (voucherID != null)
                            _userVoucherService.UpdateUserVoucherAsync(new DTO.RequestDTO.UserVoucherModel.RequestUpdateUserVoucher() { VoucherCampaignId = (int)voucherID });
                        preorderCampaign.PlacedOrderCount += cartInSameCampaign.Sum(x => x.Quantity);
                    }
                }

                requestAdminTransactionCreateModel.Money = totalAmountInOrder;
                requestCustomerTransactionCreateModel.Money = totalAmountInOrder;
                if (!await _transactionService.CreateTransaction(requestCustomerTransactionCreateModel))
                    throw new Exception("Not enough money in your wallet !");
                await _transactionService.CreateTransaction(requestAdminTransactionCreateModel);
                await _cartService.UpdateStatusOfCartByCustomerID(customerId);
                await _notificationService.CreatNotification(notificationForStaff);
                await _notificationService.CreatNotification(notificationForCustomer);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;

            }
        }

        public async Task<Pagination<ResponseOrder>> GetAllOrder(PaginationParameter page, string? searchKeyWords, string orderBy)
        {
            List<Order> orders = new List<Order>();

            if (orderBy.Equals("increase"))
            {
                orders = await _orderRepository.GetAll(filter: x => (x.ReceiverName.Contains(searchKeyWords) || x.ReceiverAddress.Contains(searchKeyWords) || String.IsNullOrEmpty(searchKeyWords))
                                                        , pagination: page, includes: x => x.OrderDetails, orderBy: x => x.OrderBy(x => x.CreatedDate));
            }
            else if (orderBy.Equals("decrease"))
            {
                orders = await _orderRepository.GetAll(filter: x => (x.ReceiverName.Contains(searchKeyWords) || x.ReceiverAddress.Contains(searchKeyWords) || String.IsNullOrEmpty(searchKeyWords))
                                                        , pagination: page, includes: x => x.OrderDetails, orderBy: x => x.OrderByDescending(x => x.CreatedDate));
            }

            var itemsOrderDetail = orders.Select(x => x.toOrderRespone()).ToList();
            var countItem = _orderRepository.Count(filter: x => (x.ReceiverName.Contains(searchKeyWords) || x.ReceiverAddress.Contains(searchKeyWords) || String.IsNullOrEmpty(searchKeyWords)));
            var result = new Pagination<ResponseOrder>(itemsOrderDetail, countItem, page.PageIndex, page.PageSize);
            return result;
        }

        public async Task<ResponseOrder> GetOrderById(int id)
        {
            var orderById = await _orderRepository.GetByIdAsync(id);
            var orderByIdResponse = orderById.toOrderRespone();

            return orderByIdResponse;
        }

        public async Task<ResponseOrder> UpdateStatusOfOrder(int orderId, RequestUpdateOrder requestUpdateOrder)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    throw new ArgumentException("Order not found");
                }
                order.Status = requestUpdateOrder.Status;
                await _orderRepository.UpdateAsync(order);
                await _unitOfWork.SaveChanges();
                await _unitOfWork.CommitTransactionAsync();
                return order.toOrderRespone();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Pagination<ResponseOrder>> OrderHistory(PaginationParameter pagination)
        {
            int customerId = _currentUserService.GetUserId();
            var orders = await _orderRepository.GetAll(filter: x => x.CustomerId == customerId, includes: x => x.OrderDetails, pagination: pagination);
            var itemsOrderDetail = orders.Select(x => x.toOrderRespone()).ToList();
            var result = new Pagination<ResponseOrder>(itemsOrderDetail, itemsOrderDetail.Count, pagination.PageIndex, pagination.PageSize);
            return result;
        }
    }
}
