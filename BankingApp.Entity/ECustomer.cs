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
    public class ECustomer
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<Customer> Add(Customer item)
        {
            item = database.AddAsync(item).Result.Entity;

            await database.SaveChangesAsync();

            return item;
        }

        public async Task<Customer> GetByIdentityNo(Customer item)
        {
            return await database.Customer.FirstOrDefaultAsync(x => x.IdentityNo.Equals(item.IdentityNo));
        }

        public async Task<Customer> GetByIdentityNoIncludeAccounts(Customer item)
        {
            return await database.Customer.Include(x=> x.Accounts).FirstOrDefaultAsync(x => x.IdentityNo.Equals(item.IdentityNo));
        }
    }
}
