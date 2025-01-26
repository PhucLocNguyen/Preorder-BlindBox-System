using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Service.Services
{
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _roleRepository;

		public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public void CreateRoleAsync(string roleName)
		{
			throw new NotImplementedException();
		}

		public void DeleteRoleByIdAsync(int roleId)
		{
			throw new NotImplementedException();
		}

		public Task<Role> GetRoleByIdAsync(int roleId)
		{
			throw new NotImplementedException();
		}

		public void UpdateRoleByIdAsync(int roleId)
		{
			throw new NotImplementedException();
		}
	}
}
