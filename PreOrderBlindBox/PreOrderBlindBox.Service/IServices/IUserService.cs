using PreOrderBlindBox.Services.DTO.RequestDTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IUserService
	{
		public Task LoginByEmailAndPassword(string email, string password);
		public Task<bool> RegisterAccountAsync(RequestRegisterAccount registerAccount);
		public Task<bool> ConfirmEmailByTokenAsync(string confirmToken);
	}
}
