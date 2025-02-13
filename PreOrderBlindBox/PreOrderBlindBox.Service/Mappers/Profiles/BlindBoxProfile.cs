using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.ImageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.Profiles
{
    public class BlindBoxProfile : Profile
    {
        public BlindBoxProfile()
        {
            // ✅ Mapping từ Image → ResponseImageModel
            CreateMap<Image, ResponseImageModel>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));

            // ✅ Mapping từ BlindBox → BlindBoxResponse
            CreateMap<BlindBox, ResponseBlindBox>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => new ResponseImageSplit
                {
                    MainImage = src.Images
                        .Where(i => i.IsMainImage)
                        .Select(i => new ResponseImageModel
                        {
                            ImageId = i.ImageId,
                            Url = i.Url,
                            IsMainImage = i.IsMainImage,
                            CreatedAt = i.CreatedAt
                        })
                        .FirstOrDefault(), // ✅ Map chính xác kiểu dữ liệu

                    GalleryImages = src.Images
                        .Where(i => !i.IsMainImage)
                        .Select(i => new ResponseImageModel
                        {
                            ImageId = i.ImageId,
                            Url = i.Url,
                            IsMainImage = i.IsMainImage,
                            CreatedAt = i.CreatedAt
                        })
                        .ToList() // ✅ Map chính xác kiểu List<ResponseImageModel>
                }));
        }
    }
}
