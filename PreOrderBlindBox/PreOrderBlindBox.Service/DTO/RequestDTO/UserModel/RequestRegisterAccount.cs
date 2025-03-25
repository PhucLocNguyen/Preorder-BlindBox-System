using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.UserModel
{
	public class RequestRegisterAccount
	{
		[Required(ErrorMessage = "Fullname is required")]
		public string FullName { get; set; }
		[Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Must be email format!")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password is required")]
		public string ConfirmPassword { get; set; }
		[Required(ErrorMessage = "Address is required")]
		public string Address { get; set; }
/*		[Required(ErrorMessage = "Role Id is required")]
		public int RoleId { get; set; }*/

	}
}
