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
        public static Order toOrderEntity(this RequestCreateOrder requestCreateOrder, int userId, int? userVoucherID)
        {
            return new Order()
            {
                Amount = (decimal)requestCreateOrder.Amount,
                DiscountMoney = (decimal)requestCreateOrder.DiscountMoney,
                CustomerId = userId,
                ReceiverName = requestCreateOrder.ReceiverName,
                ReceiverAddress = requestCreateOrder.ReceiverAddress,
                ReceiverPhone = requestCreateOrder.ReceiverPhone,
                UserVoucherId = userVoucherID,
				Status = "Confirmed",
				CreatedDate = DateTime.Now,
                UpdatedDate = null
            };
        }
        public static ResponseOrder toOrderRespone(this Order order)
        {
            return new ResponseOrder()
            {
               OrderId = order.OrderId, 
               UserVoucherId = order.UserVoucherId,
               CustomerId = order.CustomerId,
               Amount = order.Amount,
               //DiscountMoney =(decimal) order.DiscountMoney,
               CreatedDate =  order.CreatedDate.ToString("ss:mm:HH dd/MM/yyyy"),
               Receiver = order.ReceiverName,
               ReceiverPhone = order.ReceiverPhone,
               ReceiverAddress = order.ReceiverAddress,
               Status = order.Status,  
            };
        }
    }
}
