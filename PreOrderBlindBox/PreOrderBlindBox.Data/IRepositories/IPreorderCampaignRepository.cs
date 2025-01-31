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
    }
}
