using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Data.Enum;

namespace PreOrderBlindBox.Services.Services
{
    public class PreorderMilestoneService : IPreorderMilestoneService
    {
        private readonly IPreorderMilestoneRepository _preorderMilestoneRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPreorderCampaignRepository _preorderCampaignRepo;

        public PreorderMilestoneService(IPreorderMilestoneRepository preorderMilestoneRepo
            , IMapper mapper, IUnitOfWork unitOfWork
            , IPreorderCampaignRepository preorderCampaignRepo)
        {
            _preorderMilestoneRepo = preorderMilestoneRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _preorderCampaignRepo = preorderCampaignRepo;
        }

        public async Task<List<PreorderMilestone>> GetAllPreorderMilestoneByPreorderCampaignID(int preorderCampaignId)
        {
            return await _preorderMilestoneRepo.GetAll(filter: x => x.PreorderCampaignId == preorderCampaignId && x.IsDeleted == false);
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

        public async Task<int> AddPreorderMilestoneAsync(CreatePreorderMilestoneRequest createPreorderMilestoneRequest)
        {
            if (createPreorderMilestoneRequest == null)
            {
                throw new ArgumentNullException("Invalid request data");
            }

            var campaign = await _preorderCampaignRepo.GetByIdAsync(createPreorderMilestoneRequest.PreorderCampaignId);
            if (campaign == null || campaign.IsDeleted)
            {
                throw new ArgumentException("Preorder campaign not found or has been deleted.");
            }

            // Kiểm tra MilestoneNumber chỉ có thể là 1, 2 hoặc 3
            /*if (createPreorderMilestoneRequest.MilestoneNumber < 1 || createPreorderMilestoneRequest.MilestoneNumber > 3)
            {
                throw new ArgumentException("MilestoneNumber must be 1, 2, or 3.");
            }*/

            if (createPreorderMilestoneRequest.Quantity <= 0 || createPreorderMilestoneRequest.Price <= 0)
            {
                throw new ArgumentException("Quantity and Price must be larger than 0.");
            }

            // Lấy danh sách các Milestone đã có
            var existingMilestones = await GetAllPreorderMilestoneByPreorderCampaignID(createPreorderMilestoneRequest.PreorderCampaignId);

            foreach (var existMilestone in existingMilestones)
            {
                if (existMilestone.MilestoneNumber == createPreorderMilestoneRequest.MilestoneNumber)
                {
                    throw new ArgumentException("Milestone had been existed!");
                }
            }

            if (campaign.Type == PreorderCampaignType.TimedPricing.ToString())
            {
                //ValidatePreorderOneMilestone(createPreorderMilestoneRequest, existingMilestones);
                ValidatePreorderMilestone(createPreorderMilestoneRequest, existingMilestones);
            }
            else if (campaign.Type == PreorderCampaignType.BulkOrder.ToString())
            {
                //ValidatePreorderTwoMilestone(createPreorderMilestoneRequest, existingMilestones);
                ValidatePreorderMilestoneTypeTow(createPreorderMilestoneRequest, existingMilestones);
            }

            var milestone = _mapper.Map<PreorderMilestone>(createPreorderMilestoneRequest);
            await _preorderMilestoneRepo.InsertAsync(milestone);
            return await _unitOfWork.SaveChanges();

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

        public async Task<int> UpdatePreorderMilestone(int id, UpdatePreorderMilestoneRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Invalid update data");
            }

            var preorderMilestone = await _preorderMilestoneRepo.GetByIdAsync(id);
            if (preorderMilestone == null || preorderMilestone.IsDeleted)
            {
                throw new ArgumentNullException("Pre-Order Milestone does not exist or had deleted");
            }
            if (request.Quantity <= 0 || request.Price <= 0)
            {
                throw new ArgumentException("Quantity and Price must be larger than 0.");
            }

            // Lấy danh sách Milestone khác trong cùng PreorderCampaign
            var existingMilestones = await GetAllPreorderMilestoneByPreorderCampaignID(preorderMilestone.PreorderCampaignId.Value);

            // Kiểm tra loại PreorderCampaign
            var preorderCampaign = await _preorderCampaignRepo.GetByIdAsync(preorderMilestone.PreorderCampaignId.Value);
            if (preorderCampaign == null || preorderCampaign.IsDeleted)
            {
                throw new ArgumentException("Preorder campaign does not exist or had deleted");
            }

            if (preorderCampaign.Type == PreorderCampaignType.TimedPricing.ToString())
            {
                ValidateUpdatePreorderOneMilestone(preorderMilestone, request, existingMilestones);
            }
            else if (preorderCampaign.Type == PreorderCampaignType.BulkOrder.ToString())
            {
                ValidateUpdatePreorderTwoMilestone(preorderMilestone, request, existingMilestones);
            }

            // Map giá trị mới vào milestone
            _mapper.Map(request, preorderMilestone);

            await _preorderMilestoneRepo.UpdateAsync(preorderMilestone);
            return await _unitOfWork.SaveChanges();

        }

        /*public async Task<List<PreorderMilestone>> GetAllPreorderMilestoneByCampaignID(int campaignID)
        {
           return await _preorderMilestoneRepo.GetAll(filter: x=>x.PreorderCampaignId == campaignID *//*&& x.IsDeleted == false*//*);   
        }*/

        public async Task<int> CalculateRemainingQuantity(int quantityMilestone, int quantityOrderDetails)
        {
            if (quantityOrderDetails >= quantityMilestone)
            {
                return 0;
            }
            return quantityMilestone - quantityOrderDetails;
        }

        private void ValidatePreorderMilestone(CreatePreorderMilestoneRequest request, List<PreorderMilestone> existingMilestones)
        {
            // Kiểm tra mốc liền trước (nếu có)
            var previousMilestone = existingMilestones
                .Where(m => m.MilestoneNumber < request.MilestoneNumber)
                .OrderByDescending(m => m.MilestoneNumber)
                .FirstOrDefault();

            if (previousMilestone != null && request.Price <= previousMilestone.Price)
            {
                throw new ArgumentException($"Milestone {request.MilestoneNumber} phải có giá cao hơn milestone {previousMilestone.MilestoneNumber}.");
            }

            // Kiểm tra mốc liền sau (nếu có)
            var nextMilestone = existingMilestones
                .Where(m => m.MilestoneNumber > request.MilestoneNumber)
                .OrderBy(m => m.MilestoneNumber)
                .FirstOrDefault();

            if (nextMilestone != null && request.Price >= nextMilestone.Price)
            {
                throw new ArgumentException($"Milestone {request.MilestoneNumber} phải có giá thấp hơn milestone {nextMilestone.MilestoneNumber}.");
            }
        }

        private void ValidatePreorderMilestoneTypeTow(CreatePreorderMilestoneRequest request, List<PreorderMilestone> existingMilestones)
        {
            // Kiểm tra mốc liền trước (nếu có)
            var previousMilestone = existingMilestones
                .Where(m => m.MilestoneNumber < request.MilestoneNumber)
                .OrderByDescending(m => m.MilestoneNumber)
                .FirstOrDefault();

            if (previousMilestone != null && request.Price >= previousMilestone.Price)
            {
                throw new ArgumentException($"Milestone {request.MilestoneNumber} phải có giá thấp hơn milestone {previousMilestone.MilestoneNumber}.");
            }

            // Kiểm tra mốc liền sau (nếu có)
            var nextMilestone = existingMilestones
                .Where(m => m.MilestoneNumber > request.MilestoneNumber)
                .OrderBy(m => m.MilestoneNumber)
                .FirstOrDefault();

            if (nextMilestone != null && request.Price <= nextMilestone.Price)
            {
                throw new ArgumentException($"Milestone {request.MilestoneNumber} phải có giá cao hơn milestone {nextMilestone.MilestoneNumber}.");
            }
        }

        /*private void ValidatePreorderOneMilestone(CreatePreorderMilestoneRequest request, List<PreorderMilestone> existingMilestones)
        {
            // Nếu đã có Milestone 3 mà muốn tạo Milestone 1, không hợp lệ
            if (existingMilestones.Any(m => m.MilestoneNumber == 3)
                && !existingMilestones.Any(m => m.MilestoneNumber == 2)
                && request.MilestoneNumber == 1)
            {
                throw new ArgumentException("Cannot create Milestone 1 after Milestone 3 has been created.");
            }

            // Nếu đã có Milestone 1 mà muốn tạo Milestone 3, không hợp lệ
            if (existingMilestones.Any(m => m.MilestoneNumber == 1) 
                && !existingMilestones.Any(m => m.MilestoneNumber == 2)
                && request.MilestoneNumber == 3)
            {
                throw new ArgumentException("Cannot create Milestone 3 before Milestone 2.");
            }
            foreach (var milestone in existingMilestones)
            {
                if (milestone.MilestoneNumber == 1 && request.MilestoneNumber == 2)
                {
                    if (request.Price <= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 2 must have a higher price than Milestone 1.");
                    }
                }
                else if (milestone.MilestoneNumber == 2 && request.MilestoneNumber == 1)
                {
                    if (request.Price >= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 1 must have a lower price than Milestone 2.");
                    }
                }
                else if (milestone.MilestoneNumber == 2 && request.MilestoneNumber == 3)
                {
                    if (request.Price <= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 3 must have a higher price than Milestone 2.");
                    }
                }
                else if (milestone.MilestoneNumber == 3 && request.MilestoneNumber == 2)
                {
                    if (request.Price >= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 2 must have a lower price than Milestone 3.");
                    }
                }
            }
        }

        private void ValidatePreorderTwoMilestone(CreatePreorderMilestoneRequest request, List<PreorderMilestone> existingMilestones)
        {
            // Nếu đã có Milestone 3 mà muốn tạo Milestone 1, không hợp lệ
            if (existingMilestones.Any(m => m.MilestoneNumber == 3) 
                && !existingMilestones.Any(m => m.MilestoneNumber == 2)
                && request.MilestoneNumber == 1)
            {
                throw new ArgumentException("Cannot create Milestone 1 after Milestone 3 has been created.");
            }

            // Nếu đã có Milestone 1 mà muốn tạo Milestone 3, không hợp lệ
            if (existingMilestones.Any(m => m.MilestoneNumber == 1) 
                && !existingMilestones.Any(m => m.MilestoneNumber == 2)
                && request.MilestoneNumber == 3)
            {
                throw new ArgumentException("Cannot create Milestone 3 before Milestone 2.");
            }
            foreach (var milestone in existingMilestones)
            {
                if (milestone.MilestoneNumber == 1 && request.MilestoneNumber == 2)
                {
                    if (request.Price >= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 2 must have lower price than Milestone 1.");
                    }
                }
                else if (milestone.MilestoneNumber == 2 && request.MilestoneNumber == 1)
                {
                    if (request.Price <= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 1 must have higher price than Milestone 2.");
                    }
                }
                else if (milestone.MilestoneNumber == 2 && request.MilestoneNumber == 3)
                {
                    if (request.Price >= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 3 must have lower price than Milestone 2.");
                    }
                }
                else if (milestone.MilestoneNumber == 3 && request.MilestoneNumber == 2)
                {
                    if (request.Price <= milestone.Price)
                    {
                        throw new ArgumentException("Milestone 2 must have higher price than Milestone 3.");
                    }
                }
            }
        }*/

        private void ValidateUpdatePreorderOneMilestone(
            PreorderMilestone milestone,
            UpdatePreorderMilestoneRequest request,
            List<PreorderMilestone> existingMilestones)
        {
            var milestone1 = existingMilestones.FirstOrDefault(m => m.MilestoneNumber == 1);
            var milestone2 = existingMilestones.FirstOrDefault(m => m.MilestoneNumber == 2);
            var milestone3 = existingMilestones.FirstOrDefault(m => m.MilestoneNumber == 3);

            if (milestone.MilestoneNumber == 1 && milestone2 != null)
            {
                /*if (request.Quantity.HasValue && request.Quantity >= milestone2.Quantity)
                    throw new ArgumentException("Milestone 1 quantity must be lower than Milestone 2.");*/
                if (request.Price.HasValue && request.Price >= milestone2.Price)
                    throw new ArgumentException("Milestone 1 price must be lower than Milestone 2.");
            }
            else if (milestone.MilestoneNumber == 2)
            {
                /*if (milestone1 != null && request.Quantity.HasValue && request.Quantity <= milestone1.Quantity)
                    throw new ArgumentException("Milestone 2 quantity must be higher than Milestone 1.");*/
                if (milestone1 != null && request.Price.HasValue && request.Price <= milestone1.Price)
                    throw new ArgumentException("Milestone 2 price must be higher than Milestone 1.");
                /*if (milestone3 != null && request.Quantity.HasValue && request.Quantity >= milestone3.Quantity)
                    throw new ArgumentException("Milestone 2 quantity must be lower than Milestone 3.");*/
                if (milestone3 != null && request.Price.HasValue && request.Price >= milestone3.Price)
                    throw new ArgumentException("Milestone 2 price must be lower than Milestone 3.");
            }
            else if (milestone.MilestoneNumber == 3 && milestone2 != null)
            {
                /*if (request.Quantity.HasValue && request.Quantity <= milestone2.Quantity)
                    throw new ArgumentException("Milestone 3 quantity must be higher than Milestone 2.");*/
                if (request.Price.HasValue && request.Price <= milestone2.Price)
                    throw new ArgumentException("Milestone 3 price must be higher than Milestone 2.");
            }
        }

        private void ValidateUpdatePreorderTwoMilestone(
            PreorderMilestone milestone,
            UpdatePreorderMilestoneRequest request,
            List<PreorderMilestone> existingMilestones)
        {
            var milestone1 = existingMilestones.FirstOrDefault(m => m.MilestoneNumber == 1);
            var milestone2 = existingMilestones.FirstOrDefault(m => m.MilestoneNumber == 2);
            var milestone3 = existingMilestones.FirstOrDefault(m => m.MilestoneNumber == 3);

            if (milestone.MilestoneNumber == 1 && milestone2 != null)
            {
                /*if (request.Quantity.HasValue && request.Quantity >= milestone2.Quantity)
                    throw new ArgumentException("Milestone 1 quantity must be lower than Milestone 2.");*/
                if (request.Price.HasValue && request.Price <= milestone2.Price)
                    throw new ArgumentException("Milestone 1 price must be higher than Milestone 2.");
            }
            else if (milestone.MilestoneNumber == 2)
            {
                /*if (milestone1 != null && request.Quantity.HasValue && request.Quantity <= milestone1.Quantity)
                    throw new ArgumentException("Milestone 2 quantity must be higher than Milestone 1.");*/
                if (milestone1 != null && request.Price.HasValue && request.Price >= milestone1.Price)
                    throw new ArgumentException("Milestone 2 price must be lower than Milestone 1.");
                /*if (milestone3 != null && request.Quantity.HasValue && request.Quantity >= milestone3.Quantity)
                    throw new ArgumentException("Milestone 2 quantity must be lower than Milestone 3.");*/
                if (milestone3 != null && request.Price.HasValue && request.Price <= milestone3.Price)
                    throw new ArgumentException("Milestone 2 price must be higher than Milestone 3.");
            }
            else if (milestone.MilestoneNumber == 3 && milestone2 != null)
            {
                /*if (request.Quantity.HasValue && request.Quantity <= milestone2.Quantity)
                    throw new ArgumentException("Milestone 3 quantity must be higher than Milestone 2.");*/
                if (request.Price.HasValue && request.Price >= milestone2.Price)
                    throw new ArgumentException("Milestone 3 price must be lower than Milestone 2.");
            }
        }

    }
}
