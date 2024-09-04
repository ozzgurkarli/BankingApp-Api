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
    public class EAccount
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<Account> Add(Account item)
        {
            AccountTracker temp = await database.AccountTracker.FirstOrDefaultAsync(x => x.Currency.Equals(item.Currency));
            item.AccountNo = temp.FirstAvailableNo;
            database.Entry(item.Customer).State = EntityState.Unchanged;
            temp.FirstAvailableNo = (Convert.ToInt64(temp.FirstAvailableNo) + 1).ToString();
            await database.AddAsync(item);
            database.Update(temp);

            try
            {
                await database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Hatanın detaylarını loglayın veya hata mesajını gösterin
                Console.WriteLine(ex.Message);
            }

            return item;
        }


        public async Task<List<Account>> GetAll()
        {
            return await database.Account.Include(x=> x.Customer).ToListAsync();
        }
    }
}
