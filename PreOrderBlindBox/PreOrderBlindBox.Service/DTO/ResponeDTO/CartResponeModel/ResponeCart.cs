using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponeModel
{
    public class ResponeCart
    {
        public int? UserId { get; set; }
        public int? PreorderCampaignId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
