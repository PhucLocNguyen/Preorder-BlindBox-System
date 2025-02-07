using PreOrderBlindBox.Services.DTO.RequestDTO.ImageModel;
using System;
using System.Collections.Generic;
using PreOrderBlindBox.Data.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IImageService
    {
        Task<List<Image>> GetAllImageByBlindBoxID(int blindBoxId);
        Task<Image?> GetImageById(int id);
        Task<bool> UploadImage(AddImageRequest addImageRequest);
        Task<bool> DeleteImage(int imageId);
    }
}
