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
    public class EMailAddresses
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<MailAddresses> Add(MailAddresses item)
        {
            database.Entry(item.Customer).State = EntityState.Unchanged;
            await database.AddAsync(item);
            database.SaveChangesAsync();

            return item;
        }

        public Task<MailAddresses> SelectByMailAddress(MailAddresses item)
        {
            return database.MailAddress.FirstOrDefaultAsync(x => x.MailAddress.Equals(item.MailAddress));
        }
    }
}
