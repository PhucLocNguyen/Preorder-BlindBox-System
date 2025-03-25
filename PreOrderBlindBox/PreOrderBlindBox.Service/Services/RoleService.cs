using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.RoleModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.RoleModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Service.Services
{
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _roleRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_roleRepository = roleRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<int> CreateRoleAsync(string roleName)
		{
			if (String.IsNullOrEmpty(roleName))
			{
				throw new Exception("Role name cannot be empty");
			}
			Role role = new Role { RoleName = roleName };
			await _roleRepository.InsertAsync(role);
			return await _unitOfWork.SaveChanges();
		}

		public async Task<int> DeleteRoleByIdAsync(int roleId)
		{
			if (roleId <= 0)
			{
				throw new ArgumentException("Role ID must be greater than 0");
			}
			var role = await _roleRepository.GetByIdAsync(roleId);
			if (role == null)
			{
				throw new KeyNotFoundException("Role with Id not found");
			}
			await _roleRepository.Delete(role);
			return await _unitOfWork.SaveChanges();
		}

		public async Task<ResponseRole> GetRoleByIdAsync(int roleId)
		{
			if (roleId <= 0)
			{
				throw new ArgumentException("Role ID must be greater than 0");
			}
			var role = await _roleRepository.GetByIdAsync(roleId);
			if (role == null)
			{
				throw new KeyNotFoundException("Role with Id not found");
			}

			return _mapper.Map<ResponseRole>(role);
		}

		public async Task<int> UpdateRoleAsync(RequestRole requestRole)
		{
			if (requestRole == null || requestRole.RoleId <= 0)
			{
				throw new ArgumentNullException(nameof(requestRole));
			}
			var role = await _roleRepository.GetByIdAsync(requestRole.RoleId);
			_mapper.Map(requestRole, role);
			await _roleRepository.UpdateAsync(role);
			return await _unitOfWork.SaveChanges();
		}
	}
}
