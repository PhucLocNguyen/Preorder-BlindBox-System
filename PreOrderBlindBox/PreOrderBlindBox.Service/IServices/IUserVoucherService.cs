using PreOrderBlindBox.Services.DTO.RequestDTO.UserVoucherModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.UserVouchersModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IUserVoucherService
	{
		public Task<ResponseCreateUserVoucher> CreateUserVoucherAsync(RequestCreateUserVoucher userVoucher);
		public Task<int> UpdateUserVoucherAsync(RequestUpdateUserVoucher updateUserVoucher);
		public Task<int> DeleteUserVoucherAsync(int userVoucherId);
		public Task<List<ResponseUserVoucher>> GetAllUserVoucher();
		public Task<ResponseUserVoucher> GetUserVoucherById(int userVoucherId);
		public Task<ResponseUserVoucher> GetUserVoucherByVoucherCampaignId(int voucherCampaignId);
	}
}
