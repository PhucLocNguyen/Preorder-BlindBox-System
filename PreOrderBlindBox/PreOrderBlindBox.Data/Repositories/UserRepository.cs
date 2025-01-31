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
			return await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
		}
	}
}
