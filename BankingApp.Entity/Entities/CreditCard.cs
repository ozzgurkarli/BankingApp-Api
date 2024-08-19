using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class CreditCard: BaseEntity
    {
        public virtual required Customer Customer { get; set; }

        [StringLength(16)]
        public required string CardNo { get; set; }

        public decimal Limit { get; set; }

        public decimal CurrentDebt { get; set; }

        public bool Active { get; set; }

        public Int16 CVV { get; set; }

        public DateTime ExpirationDate { get; set; }

        public Int16 BillingDay { get; set; }

        public virtual required int Type { get; set; }

        public decimal OutstandingBalance { get; set; }
    }
}
