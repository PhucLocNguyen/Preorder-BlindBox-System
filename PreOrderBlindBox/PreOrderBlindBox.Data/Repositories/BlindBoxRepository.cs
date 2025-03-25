using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.Data.Commons;
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

        public async Task<List<BlindBox>> GetAllActiveBlindBox(PaginationParameter paginationParameter, string? keyword)
        {
            var items = await GetAll(paginationParameter, x => !x.IsDeleted && (x.Name.Contains(keyword)|| string.IsNullOrEmpty(keyword)), null, includes: x => x.Images);
            return items;
        }

        public async Task<BlindBox> GetDetailBlindBoxById(int id)
        {
            return await _context.BlindBoxes.Include(x => x.Images).FirstOrDefaultAsync(x => x.BlindBoxId == id);
        }

        public bool InsertBlindBox(BlindBox blindBox)
        {
            try
            {
                _context.BlindBoxes.Add(blindBox);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
