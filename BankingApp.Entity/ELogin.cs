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

        public async Task<Login> Update(Login item)
        {
            Login temp = database.Login.FirstOrDefault(x=> x.IdentityNo.Equals(item.IdentityNo));

            temp.Password = item.Password;
            temp.Temporary = item.Temporary;

            database.Update(temp);
            await database.SaveChangesAsync();

            return temp;
        }

        public async Task<Login> Select(Login item)
        {
            return await database.Login.FirstOrDefaultAsync(x => x.IdentityNo.Equals(item.IdentityNo));
        }
    }
}
