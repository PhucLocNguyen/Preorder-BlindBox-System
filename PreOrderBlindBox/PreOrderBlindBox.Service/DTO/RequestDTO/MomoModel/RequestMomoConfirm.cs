using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel
{
    public class RequestMomoConfirm
    {
        public string OrderType { get; set; }
        public int Amount { get; set; }
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
        public long TransId { get; set; }
        public long ResponseTime { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public string PayType { get; set; }
        public string RequestId { get; set; }
        public string OrderInfo { get; set; }
    }
}
