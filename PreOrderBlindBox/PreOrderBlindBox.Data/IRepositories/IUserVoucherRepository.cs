using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.IRepositories
{
	public interface IUserVoucherRepository : IGenericRepository<UserVoucher>
	{
		public Task<List<int>> GetAllCollectedVoucherCampaignIdByUserId(int userId);
		public Task<UserVoucher> GetUserVoucherByUserIdAndVoucherCampaignId(int userId, int voucherCampaignId);
		public Task<List<UserVoucher>> GetAllUserVoucher(int userId);
		public Task<UserVoucher> GetUserVoucherById(int userVoucherId);
	}
}
