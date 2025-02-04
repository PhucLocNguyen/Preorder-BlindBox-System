using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.GenericRepository
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		protected readonly Preorder_BlindBoxContext _context;
		protected readonly DbSet<TEntity> dbSet;
		public GenericRepository(Preorder_BlindBoxContext context)
		{
			_context = context;
			this.dbSet = context.Set<TEntity>();
		}

        /* public virtual IEnumerable<TEntity> Get(
			 Expression<Func<TEntity, bool>> filter = null,
			 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			 int? pageIndex = null, // Optional parameter for pagination (page number)
			 int? pageSize = null, params Expression<Func<TEntity, object>>[] includes)  // Optional parameter for pagination (number of records per page)
		 {
			 IQueryable<TEntity> query = dbSet;

			 if (filter != null)
			 {
				 query = query.Where(filter);
			 }

			 foreach (var includeProperty in includes)
			 {
				 if (includeProperty.Body is MemberExpression memberExpression)
				 {
					 query = query.Include(memberExpression.Member.Name);
				 }
			 }

			 if (orderBy != null)
			 {
				 query = orderBy(query);
			 }

			 // Implementing pagination
			 if (pageIndex.HasValue && pageSize.HasValue)
			 {
				 // Ensure the pageIndex and pageSize are valid
				 int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
				 int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

				 query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
			 }


			 return query.ToList();
		 }*/

        public Task<List<TEntity>> GetAll(
            PaginationParameter? pagination = null,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)  // Optional parameter for pagination (number of records per page)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includes)
            {
                if (includeProperty.Body is MemberExpression memberExpression)
                {
                    query = query.Include(memberExpression.Member.Name);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (pagination != null)
            {
                query = query.Skip((pagination.PageIndex - 1) * pagination.PageSize)
                    .Take(pagination.PageSize);
            }

            return query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
		{
			return await dbSet.FindAsync(id);
		}

		public virtual async Task InsertAsync(TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

            await dbSet.AddAsync(entity);
        }
        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "Entities list cannot be null or empty.");

            await dbSet.AddRangeAsync(entities);
        }

		public virtual async Task UpdateAsync(TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			dbSet.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public virtual async Task Delete(TEntity entityToDelete)
		{
			if (_context.Entry(entityToDelete).State == EntityState.Detached)
			{
				dbSet.Attach(entityToDelete);
			}
			dbSet.Remove(entityToDelete);
		}

	}
}
