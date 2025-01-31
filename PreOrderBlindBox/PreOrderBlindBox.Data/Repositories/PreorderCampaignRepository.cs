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
    public class PreorderCampaignRepository : GenericRepository<PreorderCampaign>, IPreorderCampaignRepository
    {
        public PreorderCampaignRepository(Preorder_BlindBoxContext context) : base(context)
        {
        }

        public async Task<PreorderCampaign?> GetPreorderCampaignBySlugAsync(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
                return null;

            return await _context.PreorderCampaigns.FirstOrDefaultAsync(c => c.Slug == slug);
        }
    }
}
