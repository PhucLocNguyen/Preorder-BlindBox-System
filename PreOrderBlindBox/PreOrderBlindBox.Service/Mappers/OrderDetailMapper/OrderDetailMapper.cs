using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderDetailResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.OrderDetailMapper
{
    public static class OrderDetailMapper
    {
        public static OrderDetail toOrderDetailEntity(this RequestCreateOrderDetail requestCreateOrderDetail)
        {
            return new OrderDetail()
            {
                OrderId = requestCreateOrderDetail.OrderId,
                PreorderCampaignId = requestCreateOrderDetail.PreorderCampaignId,
                Quantity = requestCreateOrderDetail.Quantity,
                UnitEndCampaignPrice = requestCreateOrderDetail.UnitEndCampaignPrice,
                UnitPriceAtTime = requestCreateOrderDetail.UnitPriceAtTime,
            };
        }

	}
}
