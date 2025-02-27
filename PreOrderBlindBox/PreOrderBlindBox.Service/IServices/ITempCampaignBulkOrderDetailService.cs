using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TempCampaignBulkOrderDetailModel;

namespace PreOrderBlindBox.Services.IServices
{
    public interface ITempCampaignBulkOrderDetailService 
    {
        Task<List<ResponseTempCampaignBulkOrderDetail>> GetAllOrderDetailsByOrderID(PaginationParameter? page, int TempCampaignBulkOrderId);
        Task<bool> CreateTempCampaignBulkOrderDetail(List<ResponseCart> carts, int TempCampaignBulkOrderId);
    }
}
