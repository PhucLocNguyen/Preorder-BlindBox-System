using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Repositories
{
	public class RoleRepository : GenericRepository<Role>, IRoleRepository
	{
		public RoleRepository(Preorder_BlindBoxContext context) : base(context)
		{
		}

		public Task<Role> GetRoleByRoleName(string roleName)
		{
			return _context.Roles.FirstOrDefaultAsync(x => x.RoleName.ToLower() == roleName.ToLower());
		}
	}
}
