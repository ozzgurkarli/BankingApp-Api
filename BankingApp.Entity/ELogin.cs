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
    public class ELogin
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<Login> Add(Login item)
        {
            await database.AddAsync(item);
            database.SaveChangesAsync();

            return item;
        }

        public Task<Login> Select(Login item)
        {
            return database.Login.FirstOrDefaultAsync(x => x.IdentityNo.Equals(item.IdentityNo));
        }
    }
}
