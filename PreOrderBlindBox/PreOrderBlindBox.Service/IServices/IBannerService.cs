
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.BannerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IBannerService
    {
        //Task<ResponseBlindBox> GetBlindBoxByIdAsync(int id);
        Task<Banner?> GetBannerById(int id);
        Task<int> CreateBanner(CreateBannerRequest request);
        Task<int> UpdateBanner(int id, UpdateBannerRequest request);
        Task<int> DeleteBanner(int id);
    }
}
