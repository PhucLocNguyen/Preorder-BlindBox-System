using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TempCampaignBulkOrderDetailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.TempCampaignBulkOrderDetailMapper
{
    public static class TempCampaignBulkOrderDetailMapper
    {
        public static TempCampaignBulkOrderDetail toTempCampaignBulkOrderDetail(this ResquestCreateTempCampaignBulkOrderDetail  requestCreateTempCampaignBulkOrderDetail)
        {
            return new TempCampaignBulkOrderDetail()
            {
                TempCampaignBulkOrderId = (int)requestCreateTempCampaignBulkOrderDetail.TempCampaignBulkOrderId,
                PreorderCampaignId = requestCreateTempCampaignBulkOrderDetail.PreorderCampaignId,
                Quantity = requestCreateTempCampaignBulkOrderDetail.Quantity,
                UnitEndCampaignPrice = requestCreateTempCampaignBulkOrderDetail.UnitEndCampaignPrice,
                UnitPriceAtTime = requestCreateTempCampaignBulkOrderDetail.UnitPriceAtTime,
            };
        }
    }
}
