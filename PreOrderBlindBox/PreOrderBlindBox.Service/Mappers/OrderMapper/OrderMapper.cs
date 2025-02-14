using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.OrderMapper
{
    public static class OrderMapper
    {
        public static Order toOrderEntity(this RequestCreateOrder requestCreateOrder, int userId)
        {
            return new Order()
            {
                Amount = (decimal)requestCreateOrder.Amount,
                CustomerId = userId,
                ReceiverName = requestCreateOrder.ReceiverName,
                ReceiverAddress = requestCreateOrder.ReceiverAddress,
                ReceiverPhone = requestCreateOrder.ReceiverPhone,
                Status = requestCreateOrder.Status,
                VoucherId = requestCreateOrder.VoucherId,
                CreatedDate = DateTime.Now,
                UpdatedDate = null
            };
        }
        public static ResponseOrder toOrderRespone(this Order order, int itemsOrderDetail)
        {
            return new ResponseOrder()
            {
               Amount = order.Amount,
               CreatedDate = order.CreatedDate,
               ReceiverAddress = order.ReceiverAddress,
               Status = order.Status,  
               TotalItems = itemsOrderDetail
            };
        }
    }
}
