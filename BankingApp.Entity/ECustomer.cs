using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity
{
    public class ECustomer
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<Customer> Add(Customer item)
        {
            item = database.AddAsync(item).Result.Entity;

            await database.SaveChangesAsync();

            return item;
        }
    }
}
