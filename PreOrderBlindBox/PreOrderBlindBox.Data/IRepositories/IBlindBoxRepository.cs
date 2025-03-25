using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.IRepositories
{
    public interface IBlindBoxRepository : IGenericRepository<BlindBox>
    {
        Task<List<BlindBox>> GetAllActiveBlindBox(PaginationParameter paginationParameter, string? keyword);
        Task<BlindBox> GetDetailBlindBoxById(int id);
        bool InsertBlindBox(BlindBox blindBox);
    }
}
