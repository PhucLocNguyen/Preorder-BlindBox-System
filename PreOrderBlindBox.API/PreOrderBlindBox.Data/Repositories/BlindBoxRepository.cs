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
    public class BlindBoxRepository : GenericRepository<BlindBox>, IBlindBoxRepository
    {
        public BlindBoxRepository(Preorder_BlindBoxContext context) : base(context)
        {
        }

        public async Task<List<BlindBox>> GetAll()
        {
            return await _context.BlindBoxes.ToListAsync();
        }
    }
}
