using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.UserVouchersModel
{
	public class ResponseUserVoucher
	{
		public int UserVoucherId { get; set; }

		public int? VoucherCampaignId { get; set; }

		public string Name { get; set; }

		public decimal PercentDiscount { get; set; }

		public decimal? MaximumMoneyDiscount { get; set; }

		public int Quantity { get; set; }

		public int UsedQuantity { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
