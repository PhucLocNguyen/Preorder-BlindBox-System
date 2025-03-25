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
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		private readonly Preorder_BlindBoxContext _context;

		public UserRepository(Preorder_BlindBoxContext context) : base(context)
		{
			_context = context;
		}

		public async Task<User?> GetUserByEmailAsync(string email)
		{
			return await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
		}

		public async Task<User?> GetUserByEmailConfirmToken(string confirmToken)
		{
			return await _context.Users.SingleOrDefaultAsync(x => x.EmailConfirmToken.ToLower() == confirmToken.ToLower());
		}

		public async Task<User?> GetUserById(int userId)
		{
			return await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == userId);
		}

		public async Task<List<User>> GetAllStaff(int roleStaffId)
		{
			return await _context.Users.Include(x => x.Role).Where(x => x.Role.RoleId == roleStaffId && x.IsActive == true && x.IsEmailConfirm == true)
				.ToListAsync();
		}

	}
}
