using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel
{
    public class RequestCreateOrder
    {
        public int CustomerId { get; set; }

        public int? VoucherId { get; set; }

        public decimal? Amount { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverPhone { get; set; }

        public string ReceiverAddress { get; set; }

        public string Status { get; set; }
    }
}
