using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.IRepositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        public Transaction AddTransaction(Transaction transaction);
        public Task<Transaction> GetDetailTransaction(int transactionId);
        public Task<(int TotalCount, List<Transaction> Transactions)> GetListOfAllTransaction(PaginationParameter paginationParameters,TypeOfTransactionEnum? type, DateTime? fromDate, DateTime? toDate, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy);
    }
}
