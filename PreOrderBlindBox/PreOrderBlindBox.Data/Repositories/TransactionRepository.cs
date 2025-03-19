using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(Preorder_BlindBoxContext context) : base(context)
        {
        }

        public Transaction AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        public async Task<Transaction> GetDetailTransaction(int transactionId)
        {
            return await _context.Transactions.Include(x => x.Wallet).ThenInclude(x=>x.Users).FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        }

        public async Task<(int TotalCount, List<Transaction> Transactions)> GetListOfAllTransaction(PaginationParameter paginationParameters, TypeOfTransactionEnum? type = null, DateTime? fromDate = null, DateTime? toDate = null, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = null)
        {
            IQueryable<Transaction> query = dbSet;
            if (fromDate != null && fromDate != DateTime.MinValue)
            {
                query = query.Where(x => x.CreatedDate >= fromDate);
            }
            if (toDate != null && toDate != DateTime.MinValue)
            {
                query = query.Where(x => x.CreatedDate <= toDate);
            }
            if (type != null && !string.IsNullOrEmpty(type.Value.ToString()))
            {
                query = query.Where(x => x.Type == type.Value.ToString());
            }
            query.Include(x => x.Wallet);
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            int totalItems = query.Count();
            if (paginationParameters != null)
            {
                query = query.Skip((paginationParameters.PageIndex - 1) * paginationParameters.PageSize)
                    .Take(paginationParameters.PageSize);
            }
            List<Transaction> transactions = await query.ToListAsync();
            return (totalItems, transactions);
        }
        public Task<List<Transaction>> GetAllFullIncludeTransaction(
    PaginationParameter? pagination = null,
    Expression<Func<Transaction, bool>>? filter = null,
    Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = null,
    params Expression<Func<Transaction, object>>[] includes)
        {
            IQueryable<Transaction> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.Include(x => x.Wallet).ThenInclude(x => x.Users);

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

    }
}
