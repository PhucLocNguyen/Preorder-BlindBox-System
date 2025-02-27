using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TempCampaignBulkOrderModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TempCampaignBulkOrderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface ITempCampaignBulkOrderService 
    {
        Task<Pagination<ResponseTempCampaignBulkOrder>> GetAllOrder(PaginationParameter page, string? searchKeyWords, string orderBy);
    }
}
