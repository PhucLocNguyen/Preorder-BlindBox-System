using AutoMapper;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TempCampaignBulkOrderDetailModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderDetailResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TempCampaignBulkOrderDetailModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.TempCampaignBulkOrderDetailMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Services
{
    public class TempCampaignBulkOrderDetailService : ITempCampaignBulkOrderDetailService
    {
        private readonly ITempCampaignBulkOrderDetailRepository _tempCampaignBulkOrderDetailRepository; 
        private readonly IPreorderCampaignService _preorderCampaignService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlindBoxService _blindBoxService;
        public TempCampaignBulkOrderDetailService(
            ITempCampaignBulkOrderDetailRepository tempCampaignBulkOrderDetailRepository,
            IPreorderCampaignService preorderCampaignService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBlindBoxService blindBoxService
            )
        {
            _tempCampaignBulkOrderDetailRepository = tempCampaignBulkOrderDetailRepository;
            _preorderCampaignService = preorderCampaignService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blindBoxService = blindBoxService;
        }
        public async Task<bool> CreateTempCampaignBulkOrderDetail(List<ResponseCart> carts, int TempCampaignBulkOrderId)
        {
            try
            {
                foreach (var item in carts)
                {
                    var preorderCampaign = await _preorderCampaignService.GetPreorderCampaignAsyncById((int)item.PreorderCampaignId);
                    var requestCreateTempCampaignBulkOrderDetail = new ResquestCreateTempCampaignBulkOrderDetail()
                    {
                        TempCampaignBulkOrderId = TempCampaignBulkOrderId,
                        PreorderCampaignId = item.PreorderCampaignId,
                        Quantity = item.Quantity,
                        UnitPriceAtTime = item.Price,
                        UnitEndCampaignPrice =  null
                    };

                    var tempCampaignBulkOrderDetail = requestCreateTempCampaignBulkOrderDetail.toTempCampaignBulkOrderDetail();
                    await _tempCampaignBulkOrderDetailRepository.InsertAsync(tempCampaignBulkOrderDetail);
                }
                await _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong when temp bulk order detail create", ex);
            }
        }

        public async Task<List<ResponseTempCampaignBulkOrderDetail>> GetAllTempOrderDetailsByTempOrderID(PaginationParameter? page, int TempCampaignBulkOrderId)
        {
            List<ResponseTempCampaignBulkOrderDetail> tempCampaignBulkOrderDetailResponse = new List<ResponseTempCampaignBulkOrderDetail>();

            var tempCampaignBulkOrderDetail = await _tempCampaignBulkOrderDetailRepository.GetAll(filter: x => x.TempCampaignBulkOrderId == TempCampaignBulkOrderId, pagination: page, includes: x => x.PreorderCampaign);
            foreach (var item in tempCampaignBulkOrderDetail)
            {
                var preorderCampaign = await _preorderCampaignService.GetPreorderCampaignAsyncById((int)item.PreorderCampaignId);
                var blindBoxResponse = _mapper.Map<ResponseBlindBox>(await _blindBoxService.GetBlindBoxByIdAsync((int)preorderCampaign.BlindBoxId));
                tempCampaignBulkOrderDetailResponse.Add(new ResponseTempCampaignBulkOrderDetail()
                {
                    TempCampaignBulkOrderDetailId = (int)item.TempCampaignBulkOrderDetailId,
                    BlindBox = blindBoxResponse,
                    Quantity = item.Quantity,
                    UnitEndCampaignPrice = item.UnitEndCampaignPrice,
                    Amount = item.Quantity * item.UnitEndCampaignPrice
                });
            }

            return tempCampaignBulkOrderDetailResponse;
        }
    }
}
