using AutoMapper;
using Azure;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.ImageModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.PreorderCampaignModel;
using PreOrderBlindBox.Services.IServices;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace PreOrderBlindBox.Services.Services
{
    public class PreorderCampaignService : IPreorderCampaignService
    {
        private readonly IPreorderCampaignRepository _preorderCampaignRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPreorderMilestoneService _preorderMilestoneService;
        private readonly IBlindBoxRepository _blindBoxRepo;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepo;

        public PreorderCampaignService(IPreorderCampaignRepository preorderCampaignRepo
            , IUnitOfWork unitOfWork
            , IPreorderMilestoneService preorderMilestoneService
            , IBlindBoxRepository blindBoxRepo
            , IMapper mapper
            , IImageRepository imageRepo)
        {
            _preorderCampaignRepo = preorderCampaignRepo;
            _unitOfWork = unitOfWork;
            _preorderMilestoneService = preorderMilestoneService;
            _blindBoxRepo = blindBoxRepo;
            _mapper = mapper;
            _imageRepo = imageRepo;
        }

        public async Task<Pagination<ResponsePreorderCampaign>> GetAllActivePreorderCampaign(PaginationParameter page)
        {
            var campaigns = await _preorderCampaignRepo.GetAllActivePreorderCampaign(page);
            var result = new List<ResponsePreorderCampaign>();

            foreach (var campaign in campaigns)
            {
                var responseCampaign = new ResponsePreorderCampaign
                {
                    Slug = campaign.Slug,
                    StartDate = campaign.StartDate,
                    EndDate = campaign.EndDate,
                    Type = campaign.Type
                };

                if (campaign.BlindBox != null)
                {
                    // Thực hiện truy vấn một cách tuần tự
                    var mainImage = await _imageRepo.GetMainImageByBlindBoxID(campaign.BlindBox.BlindBoxId);

                    responseCampaign.BlindBox = new ResponseBlindBoxWithMainImage
                    {
                        BlindBoxId = campaign.BlindBox.BlindBoxId,
                        Name = campaign.BlindBox.Name,
                        Description = campaign.BlindBox.Description,
                        Size = campaign.BlindBox.Size,
                        MainImages = mainImage != null ? new ResponseImageModel
                        {
                            ImageId = mainImage.ImageId,
                            Url = mainImage.Url
                        } : null
                    };
                }
                result.Add(responseCampaign);
            }

            return new Pagination<ResponsePreorderCampaign>(result, result.Count, page.PageIndex, page.PageSize);
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

            // Kiểm tra giá trị enum
            if (!Enum.IsDefined(typeof(PreorderCampaignType), createPreorderCampaignRequest.Type))
            {
                throw new ArgumentException("Invalid campaign type. Must be TimedPricing (0) or BulkOrder (1).");
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

        public async Task<ResponsePreorderCampaignDetail?> GetPreorderCampaignAsyncById(int id)
        {
            // Lấy thông tin PreorderCampaign từ DB
            var preorderCampaign = await _preorderCampaignRepo.GetDetailPreorderCampaignById(id);

            if (preorderCampaign == null)
            {
                return null;
            }

            // Nếu có BlindBox, lấy danh sách hình ảnh của nó
            ResponseImageSplit images = null;
            if (preorderCampaign.BlindBox != null)
            {
                var mainImage = await _imageRepo.GetMainImageByBlindBoxID(preorderCampaign.BlindBox.BlindBoxId);
                var galleryImages = await _imageRepo.GetAllImageByBlindBoxID(preorderCampaign.BlindBox.BlindBoxId);

                images = new ResponseImageSplit
                {
                    MainImage = mainImage != null ? new ResponseImageModel
                    {
                        ImageId = mainImage.ImageId,
                        Url = mainImage.Url,
                        IsMainImage = mainImage.IsMainImage,
                        CreatedAt = mainImage.CreatedAt
                    } : null,
                    GalleryImages = galleryImages
                        .Where(img => !img.IsMainImage)
                        .Select(img => new ResponseImageModel
                        {
                            ImageId = img.ImageId,
                            Url = img.Url,
                            IsMainImage = img.IsMainImage,
                            CreatedAt = img.CreatedAt
                        })
                        .ToList()
                };
            }

            // Ánh xạ sang ResponsePreorderCampaignDetail
            var response = new ResponsePreorderCampaignDetail
            {
                PreorderCampaignId = preorderCampaign.PreorderCampaignId,
                BlindBoxId = preorderCampaign.BlindBoxId,
                Slug = preorderCampaign.Slug,
                StartDate = preorderCampaign.StartDate,
                EndDate = preorderCampaign.EndDate,
                Status = preorderCampaign.Status,
                Type = preorderCampaign.Type,
                IsDeleted = preorderCampaign.IsDeleted,
                BlindBox = preorderCampaign.BlindBox != null ? new ResponseBlindBox
                {
                    BlindBoxId = preorderCampaign.BlindBox.BlindBoxId,
                    Name = preorderCampaign.BlindBox.Name,
                    Description = preorderCampaign.BlindBox.Description,
                    Size = preorderCampaign.BlindBox.Size,
                    CreatedAt = preorderCampaign.BlindBox.CreatedAt,
                    Images = images
                } : null
            };

            return response;
        }

        public async Task<ResponsePreorderCampaignDetail?> GetPreorderCampaignBySlugAsync(string slug)
        {
            var preorderCampaign = await _preorderCampaignRepo.GetPreorderCampaignBySlugAsync(slug);

            if (preorderCampaign == null)
            {
                return null;
            }
            // Nếu có BlindBox, lấy danh sách hình ảnh của nó
            ResponseImageSplit images = null;
            if (preorderCampaign.BlindBox != null)
            {
                var mainImage = await _imageRepo.GetMainImageByBlindBoxID(preorderCampaign.BlindBox.BlindBoxId);
                var galleryImages = await _imageRepo.GetAllImageByBlindBoxID(preorderCampaign.BlindBox.BlindBoxId);

                images = new ResponseImageSplit
                {
                    MainImage = mainImage != null ? new ResponseImageModel
                    {
                        ImageId = mainImage.ImageId,
                        Url = mainImage.Url,
                        IsMainImage = mainImage.IsMainImage,
                        CreatedAt = mainImage.CreatedAt
                    } : null,
                    GalleryImages = galleryImages
                        .Where(img => !img.IsMainImage)
                        .Select(img => new ResponseImageModel
                        {
                            ImageId = img.ImageId,
                            Url = img.Url,
                            IsMainImage = img.IsMainImage,
                            CreatedAt = img.CreatedAt
                        })
                        .ToList()
                };
            }

            // Ánh xạ sang ResponsePreorderCampaignDetail
            var response = new ResponsePreorderCampaignDetail
            {
                PreorderCampaignId = preorderCampaign.PreorderCampaignId,
                BlindBoxId = preorderCampaign.BlindBoxId,
                Slug = preorderCampaign.Slug,
                StartDate = preorderCampaign.StartDate,
                EndDate = preorderCampaign.EndDate,
                Status = preorderCampaign.Status,
                Type = preorderCampaign.Type,
                IsDeleted = preorderCampaign.IsDeleted,
                BlindBox = preorderCampaign.BlindBox != null ? new ResponseBlindBox
                {
                    BlindBoxId = preorderCampaign.BlindBox.BlindBoxId,
                    Name = preorderCampaign.BlindBox.Name,
                    Description = preorderCampaign.BlindBox.Description,
                    Size = preorderCampaign.BlindBox.Size,
                    CreatedAt = preorderCampaign.BlindBox.CreatedAt,
                    Images = images
                } : null
            };

            return response;
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

            if (request == null)
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

            // Kiểm tra giá trị enum
            if (!Enum.IsDefined(typeof(PreorderCampaignType), request.Type))
            {
                throw new ArgumentException("Invalid campaign type. Must be TimedPricing (0) or BulkOrder (1).");
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

        public async Task<Pagination<ResponseSearchPreorderCampaign>> SearchPreorderCampaignAsync(PreorderCampaignSearchRequest searchRequest, PaginationParameter pagination)
        {
            // Gọi repository để lấy danh sách PreorderCampaign theo yêu cầu
            var campaigns = await _preorderCampaignRepo.SearchPreorderCampaign(searchRequest.BlindBoxName, searchRequest.SortOrder.ToString(), pagination);

            var result = new List<ResponseSearchPreorderCampaign>();

            // Với mỗi campaign, thực hiện mapping và gọi riêng ImageRepository để lấy hình ảnh của BlindBox
            foreach (var campaign in campaigns)
            {
                // Tính khoảng giá dựa trên milestone của campaign
                var priceFrom = campaign.PreorderMilestones.Any()
                                    ? campaign.PreorderMilestones.Min(m => m.Price)
                                    : 0;
                var priceTo = campaign.PreorderMilestones.Any()
                                    ? campaign.PreorderMilestones.Max(m => m.Price)
                                    : 0;

                // Mapping BlindBox (không chứa hình ảnh)
                var blindBox = campaign.BlindBox;
                var responseBlindBox = _mapper.Map<ResponseBlindBox>(blindBox);

                // Lấy hình ảnh qua ImageRepository
                var mainImage = await _imageRepo.GetMainImageByBlindBoxID(blindBox.BlindBoxId);
                var galleryImages = await _imageRepo.GetAllImageByBlindBoxID(blindBox.BlindBoxId);

                responseBlindBox.Images = new ResponseImageSplit
                {
                    MainImage = mainImage != null ? new ResponseImageModel
                    {
                        ImageId = mainImage.ImageId,
                        Url = mainImage.Url,
                        IsMainImage = mainImage.IsMainImage,
                        CreatedAt = mainImage.CreatedAt
                    } : null,
                    GalleryImages = galleryImages
                        .Where(img => !img.IsMainImage)
                        .Select(img => new ResponseImageModel
                        {
                            ImageId = img.ImageId,
                            Url = img.Url,
                            IsMainImage = img.IsMainImage,
                            CreatedAt = img.CreatedAt
                        })
                        .ToList()
                };

                result.Add(new ResponseSearchPreorderCampaign
                {
                    PreorderCampaignId = campaign.PreorderCampaignId,
                    BlindBoxId = campaign.BlindBoxId,
                    Slug = campaign.Slug,
                    StartDate = campaign.StartDate,
                    EndDate = campaign.EndDate,
                    Status = campaign.Status,
                    Type = campaign.Type,
                    IsDeleted = campaign.IsDeleted,
                    BlindBox = responseBlindBox,
                    PriceFrom = priceFrom,
                    PriceTo = priceTo
                });
            }
            return new Pagination<ResponseSearchPreorderCampaign>(result, result.Count, pagination.PageIndex, pagination.PageSize);
        }

    }
}
