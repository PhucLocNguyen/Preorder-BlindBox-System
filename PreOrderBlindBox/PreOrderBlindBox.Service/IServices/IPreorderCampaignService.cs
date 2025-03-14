using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.PreorderCampaignModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IPreorderCampaignService
    {
        Task<Pagination</*PreorderCampaign*/ResponsePreorderCampaign>> GetAllValidPreorderCampaign(PaginationParameter page, PreorderCampaignGetRequest request);
        Task<Pagination<ResponsePreorderCampaign>> GetAllActivePreorderCampaign(PaginationParameter page, PreorderCampaignGetRequest request);
        Task<ResponsePreorderCampaignDetail?> GetPreorderCampaignAsyncById(int id);
        Task<ResponsePreorderCampaignDetail?> GetPreorderCampaignBySlugAsync(string slug);
        Task<bool> DeletePreorderCampaign(int id);
        Task BackGroundUpdatePreorderCampaign();
        Task<int> CancelPreorderCampaign(int id);
        Task<Pagination<ResponseSearchPreorderCampaign>> SearchPreorderCampaignAsync(PreorderCampaignSearchRequest searchRequest, PaginationParameter pagination);
        Task<bool> AddCampaignWithMilestonesAsync(CreatePreorderCampaignRequest campaignRequest);
        Task<bool> UpdatePreorderCampaignWithMilestone(int id, UpdatePreorderCampaignRequest request);
        //Task<Pagination<ResponseSearchPreorderCampaign>> FilterPreorderCampaignAsync(FilterPreorderCampaignRequest request, PaginationParameter pagination);
        Task<Pagination<ResponsePreorderCampaignDetail>> GetAllCompleteBulkCampaign(PaginationParameter pagination);
        Task<List<ResponseSearchPreorderCampaign>> GetSimilarPreorderCampaign(int id);
    }
}
