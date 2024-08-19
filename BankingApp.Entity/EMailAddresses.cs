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

        public MailAddresses Add(MailAddresses item)
        {
            database.Entry(item.Customer).State = EntityState.Unchanged;
            item = database.Add(item).Entity;
            database.SaveChanges();

            return item;
        }
    }
}
