using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Entity
{
    public class ETransactionHistory
    {
        public async Task<List<TransactionHistory>> GetAllByCustomerNoAsync(TransactionHistory item)
        {
            using (var context = new BankingDbContext())
            {
                return await context.TransactionHistory.Where(x => x.Customer.Id.Equals(item.Customer.Id)).ToListAsync();
            }
        }

        public async Task<TransactionHistory> AddAsync(TransactionHistory item)
        {
            using (var context = new BankingDbContext())
            {
                context.Entry(item.Customer).State = EntityState.Unchanged;

                if (item.CreditCard != null)        // cc and account cant be null same time
                {
                    context.Entry(item.CreditCard).State = EntityState.Unchanged;
                }
                else
                {
                    context.Entry(item.Account).State = EntityState.Unchanged;
                }
                await context.TransactionHistory.AddAsync(item);
                try{
                    await context.SaveChangesAsync();
                }
                catch(Exception ex){
                    
                }
            }

            return item;
        }
    }
}