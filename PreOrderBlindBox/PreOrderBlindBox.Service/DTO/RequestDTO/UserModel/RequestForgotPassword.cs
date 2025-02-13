using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.UserModel
{
    public class RequestForgotPassword
    {
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comfirm password is required")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
