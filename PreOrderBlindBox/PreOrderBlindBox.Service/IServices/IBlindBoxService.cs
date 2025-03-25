using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.BlindBoxModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IBlindBoxService
    {
        Task<Pagination<ResponseBlindBox>> GetAllActiveBlindBoxAsync(PaginationParameter paginationParameter, string? keyword);
        Task<ResponseBlindBox> GetBlindBoxByIdAsync(int id);
        Task<bool> CreateBlindBox(RequestCreateBlindBox request);
        Task<bool> UpdateBlindBox(int id, RequestUpdateBlindBox request);
        Task<bool> DeleteBlindBox(int id);
    }
}
