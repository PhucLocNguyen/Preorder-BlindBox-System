using PreOrderBlindBox.Services.DTO.RequestDTO.UserModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IUserService
	{
		public Task<ResponseLogin> LoginByEmailAndPasswordAsync(string email, string password);
		public Task<int> RegisterAccountAsync(RequestRegisterAccount registerAccount);
		public Task<bool> ConfirmEmailByTokenAsync(string confirmToken);
		public Task<ResponseCurrentAccountRole> GetCurrentAccountRole();
		public Task<int> ChangePassword(RequestChangePassword changePassword);
		public Task<int> ForgotPasswordForCustomer(RequestForgotPassword forgotPassword);
		public Task<int> AddNewPasswordForCustomer(RequestAddNewPassword addNewPassword);
		public Task<bool> RegisterStaffAccountAsync(RequestRegisterAccount registerStaffAccount);
		public Task<List<ResponseUserInfomation>> GetAllStaff();
		public Task<ResponseUserInfomation> GetUserById(int userId);
		public Task<int> UpdateCustomerInformation(RequestUpdateCustomerInformation updateCustomerInformation);
		public Task<int> UpdateStaffInformation(RequestUpdateStaffInformation updateStaffInformation, int staffId);
		public Task<int> DeleteStaff(int staffId);
		public Task<ResponseLogin> LoginByGoogle(string credential);
		public Task<ResponseUserInfomation> GetUserInformation();
	}
}
