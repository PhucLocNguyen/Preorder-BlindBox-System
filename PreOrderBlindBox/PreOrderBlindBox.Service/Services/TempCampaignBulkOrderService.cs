using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
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
        public TempCampaignBulkOrderService(ITempCampaignBulkOrderRepository tempCampaignBulkOrderRepository)
        {
            _tempCampaignBulkOrderRepository = tempCampaignBulkOrderRepository;
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
