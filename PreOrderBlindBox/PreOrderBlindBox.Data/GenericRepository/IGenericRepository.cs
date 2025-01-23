using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null, params Expression<Func<TEntity, object>>[] includes); // Optional parameter for pagination (number of records per page)
        Task<TEntity> GetByIdAsync(object id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
         void Delete(object id);
    }
}
