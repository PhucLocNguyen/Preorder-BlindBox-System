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
            return await _context.Transactions.Include(x => x.Wallet).FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        }

        public Task<List<Transaction>> GetListOfAllTransaction(PaginationParameter paginationParameters, TypeOfTransactionEnum? type = null, DateTime? fromDate = null, DateTime? toDate = null, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = null)
        {
            IQueryable<Transaction> query = dbSet;
            if (fromDate != DateTime.MinValue)
            {
                query = query.Where(x => x.CreatedDate >= fromDate);
            }
            if (toDate != DateTime.MinValue)
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
            if (paginationParameters != null)
            {
                query = query.Skip((paginationParameters.PageIndex - 1) * paginationParameters.PageSize)
                    .Take(paginationParameters.PageSize);
            }
            return query.ToListAsync();
            throw new NotImplementedException();
        }
    }
}
