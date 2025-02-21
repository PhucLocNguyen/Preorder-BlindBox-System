using PreOrderBlindBox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel
{
    public class ResponseOrder
    {
        public int OrderId { get; set; }
        public string CreatedDate { get; set; }
        public int TotalItems { get; set; }
        public decimal Amount { get; set; }
        public string Receiver { get; set; }
        public string ReceiverAddress { get; set; }
        public string Status { get; set; }
    }
}
