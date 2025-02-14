using AutoMapper;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Services.Services
{
    public class PreorderCampaignService : IPreorderCampaignService
    {
        private readonly IPreorderCampaignRepository _preorderCampaignRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPreorderMilestoneService _preorderMilestoneService;
        private readonly IBlindBoxRepository _blindBoxRepo;
        private readonly IMapper _mapper;

        public PreorderCampaignService(IPreorderCampaignRepository preorderCampaignRepo
            , IUnitOfWork unitOfWork
            , IPreorderMilestoneService preorderMilestoneService
            , IBlindBoxRepository blindBoxRepo
            , IMapper mapper)
        {
            _preorderCampaignRepo = preorderCampaignRepo;
            _unitOfWork = unitOfWork;
            _preorderMilestoneService = preorderMilestoneService;
            _blindBoxRepo = blindBoxRepo;
            _mapper = mapper;
        }

        public async Task<Pagination<PreorderCampaign>> GetAllPreorderCampaign(PaginationParameter page)
        {
            var preorderCampaigns = await _preorderCampaignRepo.GetAll(pagination: page);
            var result = new Pagination<PreorderCampaign>(preorderCampaigns, preorderCampaigns.Count, page.PageIndex, page.PageSize);
            return result;
        }

        public static string GenerateShortUniqueString()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                          .Replace("=", "")
                          .Replace("+", "")
                          .Replace("/", "");
        }

        public async Task<int> AddPreorderCampaignAsync(CreatePreorderCampaignRequest createPreorderCampaignRequest)
        {
            if (createPreorderCampaignRequest == null)
            {
                throw new ArgumentNullException("Invalid create data");
            }
                
            if (createPreorderCampaignRequest.EndDate < createPreorderCampaignRequest.StartDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date.");
            }

            if (createPreorderCampaignRequest.EndDate <= DateTime.Now || createPreorderCampaignRequest.StartDate < DateTime.Now)
            {
                throw new ArgumentException("Start date and end date must be in future");
            }
            if (createPreorderCampaignRequest.StartDate.AddDays(3) > createPreorderCampaignRequest.EndDate)
            {
                throw new ArgumentException("End date must be at least 3 day after start date");
            }

            var blindBox = await _blindBoxRepo.GetDetailBlindBoxById(createPreorderCampaignRequest.BlindBoxId.Value);
            if (blindBox == null || blindBox.IsDeleted)
            {
                throw new ArgumentException("Blind box does not exist or had deleted");
            }

            var preorderCampaign = new PreorderCampaign
            {
                BlindBoxId = createPreorderCampaignRequest.BlindBoxId/* ?? throw new ArgumentException("BlindBoxId is required.")*/,
                Slug = GenerateShortUniqueString(),
                StartDate = createPreorderCampaignRequest.StartDate,
                EndDate = createPreorderCampaignRequest.EndDate,
                Status = PreorderCampaignStatus.Pending.ToString(),
                Type = createPreorderCampaignRequest.Type.ToString(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false
            };

            await _preorderCampaignRepo.InsertAsync(preorderCampaign);
            return await _unitOfWork.SaveChanges();

           
        }

        public async Task<PreorderCampaign?> GetPreorderCampaignAsyncById(int id)
        {
            var preorderCampaign = await _preorderCampaignRepo.GetByIdAsync(id);

            if (preorderCampaign == null)
            {
                return null;
            }
            return preorderCampaign;
        }

        public async Task<PreorderCampaign?> GetPreorderCampaignBySlugAsync(string slug)
        {
            var preorderCampaign = await _preorderCampaignRepo.GetPreorderCampaignBySlugAsync(slug);

            if (preorderCampaign == null)
            {
                return null;
            }
            return preorderCampaign;
        }

        public async Task<bool> DeletePreorderCampaign(int id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var preorderCampaign = await _preorderCampaignRepo.GetByIdAsync(id);

                if (preorderCampaign == null)
                {
                    return false;
                }

                if (!preorderCampaign.IsDeleted)
                {
                    // Đánh dấu PreorderCampaign là đã xóa
                    preorderCampaign.IsDeleted = true;
                    await _preorderCampaignRepo.UpdateAsync(preorderCampaign);

                    // Lấy danh sách tất cả PreorderMilestone liên quan
                    var milestones = await _preorderMilestoneService.GetAllPreorderMilestoneByPreorderCampaignID(id);
                    // Đánh dấu tất cả milestones là đã xóa
                    foreach (var milestone in milestones)
                    {
                        //milestone.IsDeleted = true;
                        await _preorderMilestoneService.DeletePreorderMilestone(milestone.PreorderMilestoneId);
                    }

                    // Lưu thay đổi vào database
                    await _unitOfWork.SaveChanges();
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return false;
        }

        public async Task<int> UpdatePreorderCampaign(int id, UpdatePreorderCampaignRequest request)
        {
            var preorderCampaign = await _preorderCampaignRepo.GetByIdAsync(id);

            if (preorderCampaign == null)
            {
                throw new ArgumentException("Pre-Order Campaign not found");
            }

            if(request == null)
            {
                throw new ArgumentNullException("Invalid update Pre-Order Campaign data");
            }

            if (preorderCampaign.IsDeleted || preorderCampaign.Status == PreorderCampaignStatus.Active.ToString() 
                || preorderCampaign.Status == PreorderCampaignStatus.Completed.ToString())
            {
                throw new ArgumentException("Cannot update Pre-Order Campaign had deleted or active or completed");
            }

            if (request.EndDate < request.StartDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date.");
            }

            if (request.EndDate <= DateTime.Now || request.StartDate < DateTime.Now)
            {
                throw new ArgumentException("Start date and end date must be in future");
            }

            if (request.StartDate.AddDays(3) > request.EndDate)
            {
                throw new ArgumentException("End date must be at least 3 day after start date");
            }

            _mapper.Map(request, preorderCampaign);
            // Gọi repository để cập nhật thực thể
            await _preorderCampaignRepo.UpdateAsync(preorderCampaign);

            // Lưu thay đổi vào database
            return await _unitOfWork.SaveChanges();

        }

        public async Task BackGroundUpdatePreorderCampaign()
        {
            try
            {
                var listPreorderCampaign = await _preorderCampaignRepo.GetAllPreorderCampaign();
                if (listPreorderCampaign.Count != 0)
                {
                    var updateListPreorderCampaign = new List<PreorderCampaign>();

                    foreach (var campaign in listPreorderCampaign)
                    {
                        if (campaign.Status == PreorderCampaignStatus.Canceled.ToString() ||
                            campaign.Status == PreorderCampaignStatus.Completed.ToString())
                        {
                            continue;
                        }

                        if (campaign.StartDate <= DateTime.Now && DateTime.Now <= campaign.EndDate)
                        {
                            if (campaign.Status != PreorderCampaignStatus.Active.ToString())
                            {
                                campaign.Status = PreorderCampaignStatus.Active.ToString();
                                campaign.UpdatedDate = DateTime.Now;
                                updateListPreorderCampaign.Add(campaign);
                            }
                        }
                        else if (campaign.EndDate < DateTime.Now)
                        {
                            if (campaign.Status != PreorderCampaignStatus.Completed.ToString())
                            {
                                campaign.Status = PreorderCampaignStatus.Completed.ToString();
                                campaign.UpdatedDate = DateTime.Now;
                                updateListPreorderCampaign.Add(campaign);
                            }
                        }
                    }

                    if (updateListPreorderCampaign.Any())
                    {
                        _preorderCampaignRepo.UpdateRangeAsync(updateListPreorderCampaign);
                        await _unitOfWork.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating preorder campaigns: {ex.Message}");
            }
        }

        public async Task<int> CancelPreorderCampaign(int id, CancelPreorderCampaignRequest request)
        {
            var preorderCampaign = await _preorderCampaignRepo.GetByIdAsync(id);

            if (preorderCampaign == null)
            {
                throw new ArgumentException("Pre-Order Campaign not found");
            }

            if (request == null)
            {
                throw new ArgumentNullException("Invalid data");
            }

            if (preorderCampaign.IsDeleted || preorderCampaign.Status == PreorderCampaignStatus.Completed.ToString())
            {
                throw new ArgumentException("Cannot update Pre-Order Campaign had deleted or completed");
            }

            preorderCampaign.Status = request.Status.ToString();

            await _preorderCampaignRepo.UpdateAsync(preorderCampaign);

            // Lưu thay đổi vào database
            return await _unitOfWork.SaveChanges();
        }

    }
}
