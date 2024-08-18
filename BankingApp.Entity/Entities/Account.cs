using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class Account: BaseEntity
    {
        public required virtual Customer Customer { get; set; }

        public decimal Balance { get; set; }

        public required string CurrencyType { get; set; }

        public bool Active { get; set; }

        public virtual required Parameter Branch { get; set; }
    }
}
