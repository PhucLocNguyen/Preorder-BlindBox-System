using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.VoucherCampaignModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.Profiles
{
	public class VoucherCampaignProfile : Profile
	{
		public VoucherCampaignProfile()
		{
			CreateMap<RequestCreateVoucherCompaign, VoucherCampaign>().ReverseMap();
		}
	}
}
