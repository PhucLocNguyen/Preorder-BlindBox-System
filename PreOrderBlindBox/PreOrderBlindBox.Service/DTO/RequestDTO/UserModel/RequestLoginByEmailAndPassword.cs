using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.UserModel
{
	public class RequestLoginByEmailAndPassword
	{
		[Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Must be email format!")]
		public string Email { get; set; } = string.Empty;
		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; } = string.Empty;
	}
}
