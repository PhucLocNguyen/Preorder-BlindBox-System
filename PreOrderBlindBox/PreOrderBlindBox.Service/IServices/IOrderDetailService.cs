using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderDetailResponseModel;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IOrderDetailService
    {
        Task<List<ResponseOrderDetail>> GetAllOrderDetailsByOrderID(PaginationParameter? page, int orderId);

        Task<bool> CreateOrderDetail(List<ResponseCart> carts, int orderID);

        Task<bool> CreateTempOrderDetailToOrderDetail(List<TempCampaignBulkOrderDetail> tempCampaignBulkOrderDetails, int orderID, decimal endPriceOfCampaign);

        Task<int> GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc(int preorderCampaignID);
    }
}
