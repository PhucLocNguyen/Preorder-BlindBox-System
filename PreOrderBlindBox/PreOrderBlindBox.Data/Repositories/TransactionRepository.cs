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
            return await _context.Transactions.Include(x=>x.Wallet).FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        }
    }
}
