using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.IRepositories;

namespace PreOrderBlindBox.Data.Repositories
{
    public class TempCampaignBulkOrderDetailRepository : GenericRepository<TempCampaignBulkOrderDetail>, ITempCampaignBulkOrderDetailRepository
    {
        public TempCampaignBulkOrderDetailRepository(Preorder_BlindBoxContext context) : base(context)
        {
        }
    }
}
