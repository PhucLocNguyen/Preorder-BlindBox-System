using PreOrderBlindBox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IRoleService
	{
		public Task<Role> GetRoleByIdAsync(int roleId);
		public void CreateRoleAsync(string roleName);
		public void DeleteRoleByIdAsync(int roleId);
		public void UpdateRoleByIdAsync(int roleId);
	}
}
