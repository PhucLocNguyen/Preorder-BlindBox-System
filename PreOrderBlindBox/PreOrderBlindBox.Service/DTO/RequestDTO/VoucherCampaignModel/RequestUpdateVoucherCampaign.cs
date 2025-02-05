using PreOrderBlindBox.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.VoucherCampaignModel
{
	public class RequestUpdateVoucherCampaign
	{
		[Required(ErrorMessage = "StartDate is required")]
		[DataType(DataType.DateTime, ErrorMessage = "Invalid StartDate")]
		public DateTime StartDate { get; set; }

		[Required(ErrorMessage = "EndDate is required")]
		[DataType(DataType.DateTime, ErrorMessage = "Invalid EndDate")]
		public DateTime EndDate { get; set; }

		[Required(ErrorMessage = "PercentDiscount is required")]
		[Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
		public decimal PercentDiscount { get; set; }

		[Required(ErrorMessage = "MaximumMoneyDiscount is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Maximum money discount must be greater than 0")]
		public decimal MaximumMoneyDiscount { get; set; }

		[Required(ErrorMessage = "Quantity is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must greater than 0")]
		public int Quantity { get; set; }

		public AdminVoucherCampaignEnum? Status { get; set; }
	}
}
