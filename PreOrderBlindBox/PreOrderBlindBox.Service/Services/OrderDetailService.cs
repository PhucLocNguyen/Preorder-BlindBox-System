using AutoMapper;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderDetailResponseModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.OrderDetailMapper;

namespace PreOrderBlindBox.Services.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPreorderCampaignService _preorderCampaignService;
        private readonly IBlindBoxService _blindBoxService;
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public OrderDetailService(IOrderDetailRepository orderDetailRepository, 
            IPreorderCampaignService preorderCampaignService, IUnitOfWork unitOfWork,
            IBlindBoxService blindBoxService, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _preorderCampaignService = preorderCampaignService;
            _unitOfWork = unitOfWork;
            _blindBoxService = blindBoxService;
            _mapper = mapper;
        }

        public async Task<bool> CreateOrderDetail(List<ResponseCart> carts, int orderID)
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
                        UnitEndCampaignPrice = preorderCampaign.Type.Equals("TimedPricing") ? item.Price : null
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

        public async Task<bool> CreateTempOrderDetailToOrderDetail(List<TempCampaignBulkOrderDetail> tempCampaignBulkOrderDetails, int orderID, decimal endPriceOfCampaign)
        {
            try
            {
                foreach (var item in tempCampaignBulkOrderDetails)
                {
                    var orderDetailEntity = new OrderDetail()
                    {
                        OrderId = orderID,
                        PreorderCampaignId =item.PreorderCampaignId,
                        Quantity=item.Quantity,
                        UnitPriceAtTime = item.UnitPriceAtTime,
                        UnitEndCampaignPrice = endPriceOfCampaign
                    };
                    await _orderDetailRepository.InsertAsync(orderDetailEntity);
                }
                await _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong when change temp order detail to order detail ", ex);
            }
        }

        public async Task<List<ResponseOrderDetail>> GetAllOrderDetailsByOrderID(PaginationParameter? page, int orderId)
        {
            List<ResponseOrderDetail> orderDetailResponse = new List<ResponseOrderDetail>();

			var orderDetail = await _orderDetailRepository.GetAll(filter: x => x.OrderId == orderId, pagination: page, includes: x =>x.PreorderCampaign);
            foreach (var item in orderDetail)
            {
                var preorderCampaign = await _preorderCampaignService.GetPreorderCampaignAsyncById((int)item.PreorderCampaignId);
                var blindBoxResponse = _mapper.Map<ResponseBlindBox>(await _blindBoxService.GetBlindBoxByIdAsync((int)preorderCampaign.BlindBoxId));
                orderDetailResponse.Add(new ResponseOrderDetail()
                {
                    OrderDetailId = (int)item.OrderDetailId,
                    BlindBox = blindBoxResponse,
                    Quantity = item.Quantity,
                    UnitEndCampaignPrice = item.UnitEndCampaignPrice,
                    Amount = item.Quantity * item.UnitEndCampaignPrice
                });
			}
            
			return orderDetailResponse;
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
