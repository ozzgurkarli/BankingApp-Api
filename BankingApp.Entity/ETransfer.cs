using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Entity
{
    public class ETransfer
    {
        private readonly BankingDbContext database = new BankingDbContext();

        public async Task<Transfer> Add(Transfer item)
        {
            database.Entry(item.SenderAccount).State = EntityState.Unchanged;
            database.Entry(item.RecipientAccount).State = EntityState.Unchanged;
            database.Entry(item.SenderAccount.Customer).State = EntityState.Unchanged;
            database.Entry(item.RecipientAccount.Customer).State = EntityState.Unchanged;
            try{
            await database.AddAsync(item);

            await database.SaveChangesAsync();
            }
            catch(Exception e){
                throw e;
            }

            return item;
        }

        public async Task<List<Transfer>> UpdateAll(List<Transfer> items){
            database.Transfer.UpdateRange(items);

            await database.SaveChangesAsync();

            return items;
        }

        public async Task<List<Transfer>> GetTodayOrders(Transfer item)
        {
            return await database.Transfer.Where(x => x.OrderDate.Equals(DateTime.Today) && x.Status.Equals(1)).Include(x => x.SenderAccount).Include(x => x.RecipientAccount).ToListAsync();
        }
    }
}