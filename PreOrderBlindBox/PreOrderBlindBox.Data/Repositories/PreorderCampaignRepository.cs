using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PreOrderBlindBox.Data.Commons;
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
    public class PreorderCampaignRepository : GenericRepository<PreorderCampaign>, IPreorderCampaignRepository
    {
        public PreorderCampaignRepository(Preorder_BlindBoxContext context) : base(context)
        {
        }

        public async Task<List<PreorderCampaign>> GetAllActivePreorderCampaign(PaginationParameter paginationParameter)
        {
            var items = await GetAll(paginationParameter, x => !x.IsDeleted, null, includes: x => x.BlindBox);
            return items;
        }

        public async Task<PreorderCampaign?> GetDetailPreorderCampaignById(int id)
        {
            //return await _context.PreorderCampaigns.Include(x => x.BlindBox).FirstOrDefaultAsync(x => x.PreorderCampaignId == id);
            return await _context.PreorderCampaigns
                .Include(pc => pc.BlindBox)
                .ThenInclude(bb => bb.Images) // Lấy toàn bộ hình ảnh trong cùng một truy vấn
                .FirstOrDefaultAsync(pc => pc.PreorderCampaignId == id);
        }

        public async Task<PreorderCampaign?> GetPreorderCampaignBySlugAsync(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
                return null;

            return await _context.PreorderCampaigns.Include(pc => pc.BlindBox).ThenInclude(pc => pc.Images).FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<List<PreorderCampaign>> GetAllPreorderCampaign()
        {
            return await _context.PreorderCampaigns.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<PreorderCampaign> preorderCampaigns)
        {
            _context.PreorderCampaigns.UpdateRange(preorderCampaigns);
        }
    }
}
