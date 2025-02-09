using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.UserVouchersModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.Profiles
{
	public class UserVouchersProfile : Profile
	{
		public UserVouchersProfile()
		{
			CreateMap<UserVoucher, ResponseCreateUserVoucher>();
		}
	}
}
