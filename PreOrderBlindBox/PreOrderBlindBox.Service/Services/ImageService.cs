﻿using CurcusProject.CM.Helpers;
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
            return await _imageRepo.GetAllImageByBlindBoxID(blindBoxId);
        }

        public async Task<Image> GetMainImageIdByBlindBoxID(int blindBoxId)
        {
            return await _imageRepo.GetMainImageByBlindBoxID(blindBoxId);
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
                    IsMainImage = false
                };

                await _imageRepo.InsertAsync(image);
                uploadedImages.Add(image);
            }

            await _unitOfWork.SaveChanges();

            return true;
        }

        public async Task<bool> UploadMainImage(AddMainImageRequest addMainImageRequest)
        {
            if (addMainImageRequest.File == null)
                return false;

            var file = addMainImageRequest.File;
            var imageUrl = await _blobService.UploadFile(file);
            if (string.IsNullOrEmpty(imageUrl)) return false;

            var image = new Image
            {
                BlindBoxId = addMainImageRequest.BlindBoxId,
                Url = imageUrl,
                IsMainImage = true
            };

            await _imageRepo.InsertAsync(image);
            await _unitOfWork.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteImage(int imageId)
        {
            var image = await _imageRepo.GetByIdAsync(imageId);
            if (image == null) return false;

            await _imageRepo.Delete(image);

            // Xóa file trên Blob Storage
            var fileName = Path.GetFileName(image.Url);
            await _blobService.DeleteFile(fileName);

            await _unitOfWork.SaveChanges();
            return true;
        }
        public async Task<bool> DeleteImages(List<int> imageListId)
        {
            foreach (var imageId in imageListId)
            {
                var image = await _imageRepo.GetByIdAsync(imageId);
                if (image == null) return false;
                await _imageRepo.Delete(image);
                // Xóa file trên Blob Storage
                var fileName = Path.GetFileName(image.Url);
                await _blobService.DeleteFile(fileName);
            }
            await _unitOfWork.SaveChanges();
            return true;
        }
    }
}
