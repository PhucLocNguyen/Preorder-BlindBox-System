using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponeModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.OrderDetailMapper;

namespace PreOrderBlindBox.Services.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPreorderCampaignService _preorderCampaignService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository, 
            IPreorderCampaignService preorderCampaignService, IUnitOfWork unitOfWork)
        {
            _orderDetailRepository = orderDetailRepository;
            _preorderCampaignService = preorderCampaignService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateOrderDetail(List<ResponeCart> carts, int orderID)
        {
            try
            {
                foreach (var item in carts)
                {
                    var preorderCampaign = await _preorderCampaignService.GetPreorderCampaignAsyncById((int)item.PreorderCampaignId);
                    var requestCreateOrderDetail = new RequestCreateOrderDetail()
                    {
                        OrderId = orderID,
                        PreorderCampaignId = item.PreorderCampaignId,
                        Quantity = item.Quantity,
                        UnitPriceAtTime = item.Price,
                        UnitEndCampaignPrice = preorderCampaign.Type.Equals("Type 1") ? item.Price : null
                    };

                    var orderDetailEntity = requestCreateOrderDetail.toOrderDetailEntity();
                    await _orderDetailRepository.InsertAsync(orderDetailEntity);
                }
                await _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong when order detail create",ex);
            }
            
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsByOrderID(PaginationParameter? page, int orderId)
        {
            return await _orderDetailRepository.GetAll(filter: x => x.OrderId == orderId, pagination: page);
        }

        public async Task<int> GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc(int preorderCampaignID)
        {
            var listOrderDetailByCampaignID = await _orderDetailRepository.GetAll(
                filter: x => x.PreorderCampaignId == preorderCampaignID);
            var result = listOrderDetailByCampaignID.Sum(x => x.Quantity);
            return result;
        }
    }
}
