﻿using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity
{
    public class EAccount
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<Account> Add(Account item)
        {
            database.Entry(item.Customer).State = EntityState.Unchanged;
            await database.AddAsync(item);

            await database.SaveChangesAsync();

            return item;
        }

        public async Task<List<Account>> UpdateAll(List<Account> items){
            database.Account.UpdateRange(items);

            await database.SaveChangesAsync();

            return items;
        }

        public async Task<Account> Get(Account item)
        {
            return await database.Account.Where(x=> x.AccountNo.Equals(item.AccountNo)).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> GetAll()
        {
            return await database.Account.Include(x=> x.Customer).ToListAsync();
        }
    }
}
