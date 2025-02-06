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
	}
}
