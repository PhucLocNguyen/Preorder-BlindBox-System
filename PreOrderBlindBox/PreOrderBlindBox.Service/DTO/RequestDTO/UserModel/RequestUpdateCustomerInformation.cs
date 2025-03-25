using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.UserModel
{
	public class RequestUpdateCustomerInformation
	{
		[Required(ErrorMessage = "FullName is required")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Phone is required")]
		[RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone must be 10 digits and start with 0.")]
		public string Phone { get; set; } = string.Empty;

		[Required(ErrorMessage = "Thumbnail is required")]
		public IFormFile Thumbnail { get; set; }

		[Required(ErrorMessage = "Address is required")]
		public string Address { get; set; } = string.Empty;

		[Required(ErrorMessage = "BankName is required")]
		public string BankName { get; set; } = string.Empty;

		[Required(ErrorMessage = "BankAccountNumber is required")]
		[RegularExpression(@"^\d{9,18}$", ErrorMessage = "Bank Account Number must be between 9 and 18 digits.")]
		public string BankAccountNumber { get; set; } = string.Empty;
	}
}
