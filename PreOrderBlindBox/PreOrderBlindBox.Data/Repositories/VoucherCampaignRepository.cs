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
	public class VoucherCampaignRepository : GenericRepository<VoucherCampaign>, IVoucherCampaignRepository
	{
		public VoucherCampaignRepository(Preorder_BlindBoxContext context) : base(context)
		{
		}

		public async Task<List<VoucherCampaign>> GetAllVoucherCampaign()
		{
			return await _context.VoucherCampaigns.Where(x => x.IsDeleted == false).ToListAsync();
		}

		public async Task UpdateRangeAsync(IEnumerable<VoucherCampaign> voucherCampaigns)
		{
			_context.VoucherCampaigns.UpdateRange(voucherCampaigns);
		}
	}
}
