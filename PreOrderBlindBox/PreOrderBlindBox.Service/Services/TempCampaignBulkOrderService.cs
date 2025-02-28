using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TempCampaignBulkOrderModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TempCampaignBulkOrderModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.TempCampaignBulkOrderMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Services
{
    public class TempCampaignBulkOrderService : ITempCampaignBulkOrderService
    {
        public readonly ITempCampaignBulkOrderRepository _tempCampaignBulkOrderRepository;
        public readonly ITempCampaignBulkOrderDetailRepository _tempCampaignBulkOrderDetailRepository;
        public readonly IOrderRepository _orderRepository;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IOrderDetailService _orderDetailService;

        public TempCampaignBulkOrderService(
            ITempCampaignBulkOrderRepository tempCampaignBulkOrderRepository,
            ITempCampaignBulkOrderDetailRepository tempCampaignBulkOrderDetailRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IOrderDetailService orderDetailService
            )
        {
            _tempCampaignBulkOrderRepository = tempCampaignBulkOrderRepository;
            _tempCampaignBulkOrderDetailRepository = tempCampaignBulkOrderDetailRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _orderDetailService = orderDetailService;
        }

        public async Task<bool> convertTempCampaignBulkOrderToOrder(int preorderCampaignId, decimal endPriceOfCampaign)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var temCampaignBulkOrderByPreorderCampaignId = await _tempCampaignBulkOrderRepository.GetAll(filter: x => x.TempCampaignBulkOrderDetails.Any(d => d.PreorderCampaignId == preorderCampaignId), includes: x => x.TempCampaignBulkOrderDetails);
                if (temCampaignBulkOrderByPreorderCampaignId == null)
                    throw new Exception("Preorder campaign is not valid");
                foreach (var item in temCampaignBulkOrderByPreorderCampaignId)
                {
                    var orderEntity = new Order()
                    {
                        Amount = item.Amount,
                        DiscountMoney = (decimal)item.DiscountMoney,
                        CustomerId = item.CustomerId,
                        ReceiverName = item.ReceiverName,
                        ReceiverAddress = item.ReceiverAddress,
                        ReceiverPhone = item.ReceiverPhone,
                        UserVoucherId = item.UserVoucherId,
                        Status = "Confirm",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = null
                    };
                    await _orderRepository.InsertAsync(orderEntity);
                    await _unitOfWork.SaveChanges();
                    var temCampaignBulkOrderDetailList = await _tempCampaignBulkOrderDetailRepository.GetAll(filter: x => x.TempCampaignBulkOrderId == item.TempCampaignBulkOrderId);
                    await _orderDetailService.CreateTempOrderDetailToOrderDetail(temCampaignBulkOrderDetailList, orderEntity.OrderId, endPriceOfCampaign);
                }
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Something went wrong when convert temp campaign order to order entity", ex);
            }

        }

        public async Task<Pagination<ResponseTempCampaignBulkOrder>> GetAllOrder(PaginationParameter page, string? searchKeyWords, string orderBy)
        {
            List<TempCampaignBulkOrder> tempCampaignBulkOrder = new List<TempCampaignBulkOrder>();

            if (orderBy.Equals("increase"))
            {
                tempCampaignBulkOrder = await _tempCampaignBulkOrderRepository.GetAll(filter: x => (x.ReceiverName.Contains(searchKeyWords) || x.ReceiverAddress.Contains(searchKeyWords) || String.IsNullOrEmpty(searchKeyWords))
                                                        , pagination: page, includes: x => x.TempCampaignBulkOrderDetails, orderBy: x => x.OrderBy(x => x.CreatedDate));
            }
            else if (orderBy.Equals("decrease"))
            {
                tempCampaignBulkOrder = await _tempCampaignBulkOrderRepository.GetAll(filter: x => (x.ReceiverName.Contains(searchKeyWords) || x.ReceiverAddress.Contains(searchKeyWords) || String.IsNullOrEmpty(searchKeyWords))
                                                        , pagination: page, includes: x => x.TempCampaignBulkOrderDetails, orderBy: x => x.OrderByDescending(x => x.CreatedDate));
            }

            var itemsOrderDetail = tempCampaignBulkOrder.Select(x => x.toTempCampaignBulkOrderRespone()).ToList();
            var countItem = _tempCampaignBulkOrderRepository.Count(filter: x => (x.ReceiverName.Contains(searchKeyWords) || x.ReceiverAddress.Contains(searchKeyWords) || String.IsNullOrEmpty(searchKeyWords)));
            var result = new Pagination<ResponseTempCampaignBulkOrder>(itemsOrderDetail, countItem, page.PageIndex, page.PageSize);
            return result;
        }
    }
}
