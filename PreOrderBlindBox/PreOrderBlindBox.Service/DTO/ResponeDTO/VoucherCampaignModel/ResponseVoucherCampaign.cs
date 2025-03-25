using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.VoucherCampaignModel
{
	public class ResponseVoucherCampaign
	{
		public int VoucherCampaignId { get; set; }

		public string Name { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int Quantity { get; set; }

		public int TakenQuantity { get; set; }
		public int MaximumUserCanGet { get; set; }

		public decimal? PercentDiscount { get; set; }

		public decimal? MaximumMoneyDiscount { get; set; }

		public string Status { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
