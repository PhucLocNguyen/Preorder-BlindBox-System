using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.IRepositories
{
    public interface IPreorderCampaignRepository : IGenericRepository<PreorderCampaign>
    {
        Task<PreorderCampaign?> GetPreorderCampaignBySlugAsync(string? slug);
        Task<PreorderCampaign?> GetDetailPreorderCampaignById(int id);
        Task<List<PreorderCampaign>> GetAllPreorderCampaign();
        Task UpdateRangeAsync(IEnumerable<PreorderCampaign> preorderCampaigns);
        Task<List<PreorderCampaign>> GetAllActivePreorderCampaign(PaginationParameter paginationParameter);
        Task<List<PreorderCampaign>> SearchPreorderCampaign(
            string blindBoxName,
            string sortOrder,
            PaginationParameter paginationParameter);
        Task<List<PreorderCampaign>> FilterPreorderCampaignsAsync(
            bool isEndingSoon, bool isNewlyLaunched, bool isTrending, PaginationParameter? pagination);
        Task<List<PreorderCampaign?>> GetAllCompleteBulkPreorderCampaign(PaginationParameter paginationParameter);
    }
}
