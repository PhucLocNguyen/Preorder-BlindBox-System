using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.RoleModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.RoleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.Profiles
{
	public class RoleProfile : Profile
	{
		public RoleProfile()
		{
			CreateMap<Role, RequestRole>().ReverseMap();
			CreateMap<Role, ResponseRole>();
		}
	}
}
