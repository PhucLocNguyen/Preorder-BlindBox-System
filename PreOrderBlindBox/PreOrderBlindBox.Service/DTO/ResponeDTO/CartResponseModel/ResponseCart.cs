using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel
{
    public class ResponseCart
    {
        public int? UserId { get; set; }
        public int? PreorderCampaignId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? Amount { get; set; } = 0;
		public ResponseBlindBox BlindBox { get; set; }
	}
}
