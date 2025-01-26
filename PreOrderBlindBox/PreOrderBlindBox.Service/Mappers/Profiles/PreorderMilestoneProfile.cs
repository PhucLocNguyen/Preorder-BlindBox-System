using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.Profiles
{
    public class PreorderMilestoneProfile : Profile
    {
        public PreorderMilestoneProfile() 
        {
            CreateMap<CreatePreorderMilestoneRequest, PreorderMilestone>();
            CreateMap<UpdatePreorderMilestoneRequest, PreorderMilestone>();
        }
    }
}
