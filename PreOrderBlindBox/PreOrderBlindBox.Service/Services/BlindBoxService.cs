using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.BlindBoxModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.ImageModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Services.Services
{
    public class BlindBoxService : IBlindBoxService
    {
        private readonly IBlindBoxRepository _blindBoxRepository;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPreorderCampaignRepository _preorderCampaignRepository;
        public BlindBoxService(IBlindBoxRepository blindBoxRepository, IImageService imageService, IUnitOfWork unitOfWork, IMapper mapper, IPreorderCampaignRepository preorderCampaignRepository)
        {
            _blindBoxRepository = blindBoxRepository;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _preorderCampaignRepository = preorderCampaignRepository;
        }

        public async Task<bool> CreateBlindBox(RequestCreateBlindBox request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var blindbox = new BlindBox
                {
                    Name = request.Name,
                    Description = request.Description,
                    IsDeleted = false,
                    Size = request.Size,
                    CreatedAt = DateTime.Now,
                    ListedPrice = Decimal.Parse(request.listedPrice) 
                };
                _blindBoxRepository.InsertBlindBox(blindbox);

                if (request.MainImage != null)
                {
                    var mainImageUpdate = new AddMainImageRequest()
                    {
                        BlindBoxId = blindbox.BlindBoxId,
                        File = request.MainImage
                    };

                    await _imageService.UploadMainImage(mainImageUpdate);
                }
                else
                {
                    throw new Exception("Không thể tải lên ảnh chính");
                }
                if (!request.GalleryImages.IsNullOrEmpty() && request.GalleryImages?.Count > 0)
                {
                    var galleryImagesUpdate = new AddImageRequest()
                    {
                        BlindBoxId = blindbox.BlindBoxId,
                        Files = request.GalleryImages
                    };
                    await _imageService.UploadImage(galleryImagesUpdate);
                }

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<ResponseBlindBox> GetBlindBoxByIdAsync(int id)
        {
            BlindBox blindBox = await _blindBoxRepository.GetDetailBlindBoxById(id);
            if (blindBox == null)
            {
                return null;
            }
            ResponseBlindBox responseBlindBox = _mapper.Map<ResponseBlindBox>(blindBox);

            return responseBlindBox;

        }

        public async Task<Pagination<ResponseBlindBox>> GetAllActiveBlindBoxAsync(PaginationParameter paginationParameter, string? keyword)
        {
            List<BlindBox> listBlindBox = await _blindBoxRepository.GetAllActiveBlindBox(paginationParameter, keyword);
            List<ResponseBlindBox> responseMap = _mapper.Map<List<ResponseBlindBox>>(listBlindBox);
            var countItem = _blindBoxRepository.Count(x => x.IsDeleted == false);

            var responseData = new Pagination<ResponseBlindBox>(responseMap, countItem, paginationParameter.PageIndex, paginationParameter.PageSize);
            return responseData;
        }

        public async Task<bool> UpdateBlindBox(int id, RequestUpdateBlindBox request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var blindbox = await _blindBoxRepository.GetByIdAsync(id);
                if (blindbox == null)
                {
                    return false;
                }
                blindbox.Name = request.Name;
                blindbox.Description = request.Description;
                blindbox.Size = request.Size;
                blindbox.ListedPrice = decimal.Parse(request.listedPrice);
                if (request.MainImage != null)
                {
                    // Neu co hinh anh moi
                    var oldMainImage = await _imageService.GetMainImageIdByBlindBoxID(blindbox.BlindBoxId);
                    if (oldMainImage != null)
                    {
                        // Xoa hinh anh cu
                        await _imageService.DeleteImage(oldMainImage.ImageId);
                    }
                    // Them hinh anh moi
                    var mainImageUpdate = new AddMainImageRequest()
                    {
                        BlindBoxId = blindbox.BlindBoxId,
                        File = request.MainImage
                    };

                    bool mainImageUploaded = await _imageService.UploadMainImage(mainImageUpdate);
                    if (!mainImageUploaded)
                    {
                        throw new Exception("Không thể tải lên ảnh chính");
                    }
                }
                if (!request.DeletedGalleryImagesID.IsNullOrEmpty() && request.DeletedGalleryImagesID?.Count > 0)
                {
                    await _imageService.DeleteImages(request.DeletedGalleryImagesID);
                }

                if (!request.GalleryImages.IsNullOrEmpty() && request.GalleryImages?.Count > 0)
                {
                    var galleryImagesUpdate = new AddImageRequest()
                    {
                        BlindBoxId = blindbox.BlindBoxId,
                        Files = request.GalleryImages
                    };
                    await _imageService.UploadImage(galleryImagesUpdate);
                }
                // Update hinh anh tu image service ....
                await _blindBoxRepository.UpdateAsync(blindbox);
                await _unitOfWork.SaveChanges();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteBlindBox(int id)
        {
            try
            {
                var blindbox = await _blindBoxRepository.GetByIdAsync(id);
                if (blindbox == null)
                {
                    return false;
                }
                var items = _preorderCampaignRepository.Count(x => x.BlindBoxId == id && x.Status.Equals(PreorderCampaignStatus.Active));
                if (items > 0)
                {
                    return false;
                }
                blindbox.IsDeleted = true;
                await _blindBoxRepository.UpdateAsync(blindbox);
                await _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }
    }
}
