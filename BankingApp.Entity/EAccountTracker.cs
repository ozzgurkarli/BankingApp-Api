using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Entity
{
    public class EAccountTracker
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<AccountTracker> GetAndIncrease(AccountTracker acc)
        {
            AccountTracker item = await database.AccountTracker.Where(x=> x.Currency.Equals(acc.Currency)).FirstOrDefaultAsync();

            item.FirstAvailableNo = (UInt64.Parse(item.FirstAvailableNo!) + 1).ToString();

            database.Update(item);
            await database.SaveChangesAsync();

            item.FirstAvailableNo = (UInt64.Parse(item.FirstAvailableNo!) - 1).ToString();
            return item;
        }

    }
}