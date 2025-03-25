using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel
{
    public class RequestCreateOrderDetail
    {
        public int? OrderId { get; set; }

        public int? PreorderCampaignId { get; set; }

        public decimal UnitPriceAtTime { get; set; }

        public int Quantity { get; set; }

        public decimal? UnitEndCampaignPrice { get; set; }
    }
}
