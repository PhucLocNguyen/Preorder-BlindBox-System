using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.RoleModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.RoleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IRoleService
	{
		public Task<ResponseRole> GetRoleByIdAsync(int roleId);
		public Task<int> CreateRoleAsync(string roleName);
		public Task<int> DeleteRoleByIdAsync(int roleId);
		public Task<int> UpdateRoleAsync(RequestRole role);
	}
}
