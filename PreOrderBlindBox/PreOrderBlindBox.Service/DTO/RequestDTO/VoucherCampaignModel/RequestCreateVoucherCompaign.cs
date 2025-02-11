using PreOrderBlindBox.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.VoucherCampaignModel
{
    public class RequestCreateVoucherCompaign
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid StartDate")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid EndDate")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "MaximumUserCanGet is required")]
        [Range(1, int.MaxValue, ErrorMessage = "MaximumUserCanGet must greater than 0")]
        public int MaximumUserCanGet { get; set; }

        [Required(ErrorMessage = "PercentDiscount is required")]
        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
        public decimal PercentDiscount { get; set; }

        [Required(ErrorMessage = "MaximumMoneyDiscount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Maximum money discount must be greater than 0")]
        public decimal MaximumMoneyDiscount { get; set; }

        [Required(ErrorMessage = "SetNumberExpirationDate is required")]
        [Range(1, 100, ErrorMessage = "Set number expiration date must be between 1 and 100")]
        public int SetNumberExpirationDate { get; set; }

        /*[Required(ErrorMessage = "MaximumDiscount is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Maximum discount must be greater than 0")]
		public decimal MaximumDiscount { get; set; }*/

        /*[Required(ErrorMessage = "Status is required")]
		public VoucherCampaignEnum Status { get; set; }*/
    }
}
