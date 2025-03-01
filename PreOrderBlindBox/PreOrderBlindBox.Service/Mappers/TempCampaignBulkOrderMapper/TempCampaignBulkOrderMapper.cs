using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TempCampaignBulkOrderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.TempCampaignBulkOrderMapper
{
    public static class TempCampaignBulkOrderMapper
    {
        public static TempCampaignBulkOrder toTempCampaignBulkOrder(this RequestCreateOrder requestCreateOrder, int userId, int? userVoucherID)
        {
            return new TempCampaignBulkOrder()
            {
                Amount = (decimal)requestCreateOrder.Amount,
                CustomerId = userId,
                ReceiverName = requestCreateOrder.ReceiverName,
                ReceiverAddress = requestCreateOrder.ReceiverAddress,
                ReceiverPhone = requestCreateOrder.ReceiverPhone,
                UserVoucherId = userVoucherID,
                Status = requestCreateOrder.Status,
                CreatedDate = DateTime.Now,
                UpdatedDate = null
            };
        }
        public static ResponseTempCampaignBulkOrder toTempCampaignBulkOrderRespone(this TempCampaignBulkOrder tempCampaignBulkOrder)
        {
            return new ResponseTempCampaignBulkOrder()
            {
                TempCampaignBulkOrderId = tempCampaignBulkOrder.TempCampaignBulkOrderId,
                UserVoucherId = tempCampaignBulkOrder.UserVoucherId,
                CustomerId = tempCampaignBulkOrder.CustomerId,
                Amount = tempCampaignBulkOrder.Amount,
                CreatedDate = tempCampaignBulkOrder.CreatedDate.ToString("dd MMM, yyyy"),
                Receiver = tempCampaignBulkOrder.ReceiverName,
                ReceiverPhone = tempCampaignBulkOrder.ReceiverPhone,
                ReceiverAddress = tempCampaignBulkOrder.ReceiverAddress,
                Status = tempCampaignBulkOrder.Status,
                TotalItems = tempCampaignBulkOrder.TempCampaignBulkOrderDetails.Sum(x => x.Quantity),
            };
        }
    }
}
