using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
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

        public Login Add(Login item)
        {
            item = database.Add(item).Entity;
            database.SaveChanges();

            return item;
        }
    }
}
