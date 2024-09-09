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
            database.ChangeTracker.AutoDetectChangesEnabled = false;
            database.Entry(item.SenderAccount).State = EntityState.Unchanged;
            database.Entry(item.RecipientAccount).State = EntityState.Unchanged;

            await database.AddAsync(item);

            await database.SaveChangesAsync();

            return item;
        }

        public async Task<Transfer> Update(Transfer item)
        {
            using (var context = new BankingDbContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.Entry(item.SenderAccount).State = EntityState.Unchanged;
                context.Entry(item.RecipientAccount).State = EntityState.Unchanged;
                context.Transfer.UpdateRange(item);

                await context.SaveChangesAsync();
            }

            return item;
        }

        public async Task<List<Transfer>> GetTodayOrders(Transfer item)
        {
            return await database.Transfer.Where(x => x.OrderDate.Equals(DateTime.Today) && x.Status.Equals(1)).Include(x => x.SenderAccount).Include(x => x.RecipientAccount).ToListAsync();
        }
    }
}