using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IPreorderCampaignService
    {
        Task<Pagination<PreorderCampaign>> GetAllPreorderCampaign(PaginationParameter page);
        Task<int> AddPreorderCampaignAsync(CreatePreorderCampaignRequest createPreorderCampaignRequest);
        Task<PreorderCampaign?> GetPreorderCampaignAsyncById(int id);
        Task<PreorderCampaign?> GetPreorderCampaignBySlugAsync(string slug);
        Task<bool> DeletePreorderCampaign(int id);
        Task<int> UpdatePreorderCampaign(int id, UpdatePreorderCampaignRequest request);
        Task BackGroundUpdatePreorderCampaign();
    }
}
