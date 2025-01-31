using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
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

        public PreorderCampaignService(IPreorderCampaignRepository preorderCampaignRepo
            , IUnitOfWork unitOfWork
            , IPreorderMilestoneService preorderMilestoneService)
        {
            _preorderCampaignRepo = preorderCampaignRepo;
            _unitOfWork = unitOfWork;
            _preorderMilestoneService = preorderMilestoneService;
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

        public async Task<PreorderCampaign> AddPreorderCampaignAsync(CreatePreorderCampaignRequest createPreorderCampaignRequest)
        {
            if (createPreorderCampaignRequest == null)
                throw new ArgumentNullException(nameof(createPreorderCampaignRequest));

            if (createPreorderCampaignRequest.EndDate < createPreorderCampaignRequest.StartDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date.");
            }

            var preorderCampaign = new PreorderCampaign
            {
                BlindBoxId = createPreorderCampaignRequest.BlindBoxId ?? throw new ArgumentException("BlindBoxId is required."),
                Slug = GenerateShortUniqueString(),
                StartDate = createPreorderCampaignRequest.StartDate,
                EndDate = createPreorderCampaignRequest.EndDate,
                Status = createPreorderCampaignRequest.Status.ToString(),
                Type = createPreorderCampaignRequest.Type.ToString(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            await _preorderCampaignRepo.InsertAsync(preorderCampaign);
            await _unitOfWork.SaveChanges();

            return preorderCampaign;
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
            /*var preorderCampaign = await _preorderCampaignRepo.GetByIdAsync(id);

            if (preorderCampaign == null)
            {
                return false;
            }

            if (!preorderCampaign.IsDeleted)
            {
                preorderCampaign.IsDeleted = true;
                await _preorderCampaignRepo.UpdateAsync(preorderCampaign);
                await _unitOfWork.SaveChanges();
                return true;
            }
            return false;*/
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

        public async Task<PreorderCampaign?> UpdatePreorderCampaign(int id, UpdatePreorderCampaignRequest request)
        {
            var preorderCampaign = await _preorderCampaignRepo.GetByIdAsync(id);

            if (preorderCampaign == null)
            {
                return null;
            }

            if (request.StartDate.HasValue && request.EndDate.HasValue &&
                request.EndDate < request.StartDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date.");
            }

            if (request.StartDate.HasValue)
            {
                preorderCampaign.StartDate = request.StartDate.Value;
            }

            if (request.EndDate.HasValue)
            {
                preorderCampaign.EndDate = request.EndDate.Value;
            }

            if (request.Status.HasValue)
            {
                preorderCampaign.Status = request.Status.Value.ToString();
            }

            if (request.Type.HasValue)
            {
                preorderCampaign.Type = request.Type.Value.ToString();
            }

            // Gọi repository để cập nhật thực thể
            await _preorderCampaignRepo.UpdateAsync(preorderCampaign);

            // Lưu thay đổi vào database
            await _unitOfWork.SaveChanges();

            return preorderCampaign;  // Trả về chiến dịch đã cập nhật

        }
    }
}
