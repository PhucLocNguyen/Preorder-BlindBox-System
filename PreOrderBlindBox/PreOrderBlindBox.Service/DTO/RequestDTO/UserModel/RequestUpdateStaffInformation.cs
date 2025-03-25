using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.UserModel
{
	public class RequestUpdateStaffInformation
	{
		[Required(ErrorMessage = "FullName is required")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Phone is required")]
		public string Phone { get; set; } = string.Empty;

		[Required(ErrorMessage = "Thumbnail is required")]
		public IFormFile Thumbnail { get; set; }

		[Required(ErrorMessage = "Address is required")]
		public string Address { get; set; } = string.Empty;

	}
}
