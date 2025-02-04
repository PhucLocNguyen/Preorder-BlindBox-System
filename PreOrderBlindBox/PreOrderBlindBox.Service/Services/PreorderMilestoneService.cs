using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Services.Services
{
    public class PreorderMilestoneService : IPreorderMilestoneService
    {
        private readonly IPreorderMilestoneRepository _preorderMilestoneRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PreorderMilestoneService(IPreorderMilestoneRepository preorderMilestoneRepo
            , IMapper mapper, IUnitOfWork unitOfWork)
        {
            _preorderMilestoneRepo = preorderMilestoneRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PreorderMilestone>> GetAllPreorderMilestoneByPreorderCampaignID(int preorderCampaignId)
        {
            return await _preorderMilestoneRepo.GetAll(filter: x => x.PreorderCampaignId == preorderCampaignId);
        }

        public async Task<PreorderMilestone?> GetPreorderMilestoneById(int id)
        {
            var result = await _preorderMilestoneRepo.GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<PreorderMilestone> AddPreorderMilestoneAsync(CreatePreorderMilestoneRequest createPreorderMilestoneRequest)
        {
            var preorderMilestoneMapper = _mapper.Map<PreorderMilestone>(createPreorderMilestoneRequest);

            await _preorderMilestoneRepo.InsertAsync(preorderMilestoneMapper);
            await _unitOfWork.SaveChanges();
            return preorderMilestoneMapper;
        }

        public async Task<bool> DeletePreorderMilestone(int id)
        {
            var preorderMilestone = await _preorderMilestoneRepo.GetByIdAsync(id);

            if (!preorderMilestone.IsDeleted)
            {
                preorderMilestone.IsDeleted = true;
                await _preorderMilestoneRepo.UpdateAsync(preorderMilestone);
                await _unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<PreorderMilestone?> UpdatePreorderMilestone(int id, UpdatePreorderMilestoneRequest request)
        {
            var preorderMilestone = await _preorderMilestoneRepo.GetByIdAsync(id);

            if (preorderMilestone == null) return null;

            _mapper.Map(request, preorderMilestone);

            await _preorderMilestoneRepo.UpdateAsync(preorderMilestone);
            await _unitOfWork.SaveChanges();

            return preorderMilestone;
        }

        public async Task<List<PreorderMilestone>> GetAllPreorderMilestoneByCampaignID(int campaignID)
        {
           return await _preorderMilestoneRepo.GetAll(filter: x=>x.PreorderCampaignId == campaignID);   
        }

        public async Task<int> CalculateRemainingQuantity(int milestoneID, int quantityOrderDetails)
        {
            var milestone = await GetPreorderMilestoneById(milestoneID);
            if (quantityOrderDetails >= milestone.Quantity)
            {
                return 0;
            }
            return milestone.Quantity - quantityOrderDetails;
        }
    }
}
