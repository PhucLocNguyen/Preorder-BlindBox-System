using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.OrderMapper
{
    public static class OrderMapper
    {
        public static Order toOrderEntity(this RequestCreateOrder requestCreateOrder)
        {
            return new Order()
            {
                Amount = (decimal)requestCreateOrder.Amount,
                CustomerId = requestCreateOrder.CustomerId,
                ReceiverName = requestCreateOrder.ReceiverName,
                ReceiverAddress = requestCreateOrder.ReceiverAddress,
                ReceiverPhone = requestCreateOrder.ReceiverPhone,
                Status = requestCreateOrder.Status,
                VoucherId = requestCreateOrder.VoucherId,
                CreatedDate = DateTime.Now,
                UpdatedDate = null
            };
        }
    }
}
