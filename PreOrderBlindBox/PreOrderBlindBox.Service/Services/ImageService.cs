using CurcusProject.CM.Helpers;
using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.ImageModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Services.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepo;
        private readonly IBlobService _blobService;
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IImageRepository imageRepo, IBlobService blobService, IUnitOfWork unitOfWork)
        {
            _imageRepo = imageRepo;
            _blobService = blobService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Image>> GetAllImageByBlindBoxID(int blindBoxId)
        {
            return await _imageRepo.GetAll(filter: x => x.BlindBoxId == blindBoxId);
        }

        public async Task<Image?> GetImageById(int id)
        {
            var result = await _imageRepo.GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> UploadImage(AddImageRequest addImageRequest)
        {
            if (addImageRequest.Files == null || addImageRequest.Files.Count == 0)
                return false;

            var uploadedImages = new List<Image>();

            for (int i = 0; i < addImageRequest.Files.Count; i++)
            {
                var file = addImageRequest.Files[i];
                var imageUrl = await _blobService.UploadFile(file);
                if (string.IsNullOrEmpty(imageUrl)) continue;

                var image = new Image
                {
                    BlindBoxId = addImageRequest.BlindBoxId,
                    Url = imageUrl,
                    Title = file.FileName,
                    IsMainImage = (bool)addImageRequest.IsMainImage[i],  // Nhận trạng thái từ frontend
                };

                await _imageRepo.InsertAsync(image);
                //_context.Images.Add(image);
                uploadedImages.Add(image);
            }

            await _unitOfWork.SaveChanges();

            //return uploadedImages;
            return true;
        }

        public async Task<bool> DeleteImage(int imageId)
        {
            var image = await _imageRepo.GetByIdAsync(imageId);
            if (image == null || image.IsDeleted) return false;

            image.IsDeleted = true;
            await _imageRepo.UpdateAsync(image);
            //await _context.SaveChangesAsync();

            // Xóa file trên Blob Storage
            var fileName = Path.GetFileName(image.Url);
            await _blobService.DeleteFile(fileName);

            await _unitOfWork.SaveChanges();
            return true;
        }
    }
}
