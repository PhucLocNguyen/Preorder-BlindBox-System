using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.UserModel
{
	public class RequestAddNewPassword
	{
		[Required(ErrorMessage = "New password is required")]
		public string NewPassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "Confirm new password is required")]
		public string ConfirmNewPassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "Forgot password token is required")]
		public string ForgotPasswordToken { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Must be email format!")]
		public string Email { get; set; } = string.Empty;
	}
}
