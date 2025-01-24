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
    }
}
