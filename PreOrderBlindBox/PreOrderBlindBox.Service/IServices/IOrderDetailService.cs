using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetail>> GetAllOrderDetailsByOrderID(PaginationParameter? page, int orderId);

        Task<bool> CreateOrderDetail(List<ResponseCart> carts, int orderID);

        Task<int> GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc(int preorderCampaignID);
    }
}
