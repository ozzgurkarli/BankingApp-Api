using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity
{
    public class ECreditCard
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<List<CreditCard>> GetAll()
        {
            return await database.CreditCard.Include(x=> x.Customer).ToListAsync();
        }

        public async Task<CreditCard> Add(CreditCard cc){
            database.Entry(cc.Customer).State = EntityState.Unchanged;
            await database.AddAsync(cc);
            await database.SaveChangesAsync();

            return cc;
        }
    }
}
