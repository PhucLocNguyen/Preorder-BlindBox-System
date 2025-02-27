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
        public static ResponseOrder toOrderRespone(this Order order)
        {
            return new ResponseOrder()
            {
               OrderId = order.OrderId, 
               UserVoucherId = order.UserVoucherId,
               CustomerId = order.CustomerId,
               Amount = order.Amount,
               CreatedDate =  order.CreatedDate.ToString("dd MMM, yyyy"),
               Receiver = order.ReceiverName,
               ReceiverPhone = order.ReceiverPhone,
               ReceiverAddress = order.ReceiverAddress,
               Status = order.Status,  
               TotalItems = order.OrderDetails.Sum(x=>x.Quantity),
            };
        }
    }
}
