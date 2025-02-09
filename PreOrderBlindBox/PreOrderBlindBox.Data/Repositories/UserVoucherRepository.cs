using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Repositories
{
	public class UserVoucherRepository : GenericRepository<UserVoucher>, IUserVoucherRepository
	{
		public UserVoucherRepository(Preorder_BlindBoxContext context) : base(context)
		{
		}

		public async Task<List<int>> GetAllCollectedVoucherCampaignIdByUserId(int userId)
		{
			var listUserVoucher = await _context.UserVouchers.Where(x => x.UserId == userId && x.VoucherCampaignId.HasValue)
				.Select(x => x.VoucherCampaignId.Value)
				.ToListAsync();
			return listUserVoucher ?? [];
		}

		public async Task<UserVoucher> GetUserVoucherByUserIdAndVoucherCampaignId(int userId, int voucherCampaignId)
		{
			UserVoucher userVoucher = await _context.UserVouchers.FirstOrDefaultAsync(x => x.UserId == userId && x.VoucherCampaignId == voucherCampaignId);
			return userVoucher;
		}

		public async Task<List<UserVoucher>> GetAllUserVoucher(int userId)
		{
			var result = await _context.UserVouchers.Include(uv => uv.VoucherCampaign)
				.Where(uv => uv.UserId == userId && uv.UsedQuantity < uv.Quantity && uv.VoucherCampaign != null && DateTime.Now < uv.VoucherCampaign.ExpiredDate)
				.ToListAsync();
			return result ?? new List<UserVoucher>();
		}

		public async Task<UserVoucher> GetUserVoucherById(int userVoucherId)
		{
			var result = await _context.UserVouchers.Include(uv => uv.VoucherCampaign)
				.FirstOrDefaultAsync(uv => uv.UserVoucherId == userVoucherId);
			return result;
		}
	}
}
