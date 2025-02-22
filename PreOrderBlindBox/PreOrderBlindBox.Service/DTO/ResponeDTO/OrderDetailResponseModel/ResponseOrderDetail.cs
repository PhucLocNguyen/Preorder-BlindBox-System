using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.OrderDetailResponseModel
{
	public class ResponseOrderDetail
	{
		public int OrderDetailId { get; set; }
		public ResponseBlindBox BlindBox { get; set; }
		public int Quantity { get; set; }
		public decimal? UnitEndCampaignPrice { get; set; }
		public decimal? Amount { get; set; }
	}
}
