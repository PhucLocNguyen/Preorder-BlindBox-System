using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TempCampaignBulkOrderModel;

namespace PreOrderBlindBox.Services.IServices
{
    public interface ITempCampaignBulkOrderService 
    {
        Task<Pagination<ResponseTempCampaignBulkOrder>> GetAllTempOrder(PaginationParameter page, string? searchKeyWords, string orderBy);
        Task<Pagination<ResponseTempCampaignBulkOrder>> TempOrderHistory(PaginationParameter pagination);
        Task<bool> ConvertTempCampaignBulkOrderToOrder(int preorderCampaignId, decimal endPriceOfCampaign);
        Task<ResponseTempCampaignBulkOrder> GetTempOrderById(int id);
        Task<TempCampaignBulkOrder> CreateOrder(TempCampaignBulkOrder tempCampaignBulkOrder);
    }
}
