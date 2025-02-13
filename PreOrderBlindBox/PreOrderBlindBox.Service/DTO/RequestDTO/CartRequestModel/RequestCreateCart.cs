using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel
{
    public class RequestCreateCart
    {
        public int? PreorderCampaignId { get; set; }
        public int Quantity { get; set; }
    }
}
