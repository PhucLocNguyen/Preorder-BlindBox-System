using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.Profiles
{
    public class PreorderCampaignProfile : Profile
    {
        public PreorderCampaignProfile()
        {
            CreateMap<CreatePreorderCampaignRequest, PreorderCampaign>();
            CreateMap<UpdatePreorderCampaignRequest, PreorderCampaign>();
        }
    }
}
