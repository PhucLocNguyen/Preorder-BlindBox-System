using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.UserModel
{
	public class ResponseUserInfomation
	{
		public int UserId { get; set; }
		public string FullName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Thumbnail { get; set; }
		public string BankName { get; set; }
		public string BankAccountNumber { get; set; }
		public bool IsActive { get; set; }

	}
}
