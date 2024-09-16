using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class TransactionHistory: BaseEntity
    {
        public virtual Customer? Customer { get; set; }

        public virtual Account? Account { get; set; }

        public virtual CreditCard? CreditCard { get; set; }

        public decimal? Amount { get; set; }

        public required string Currency { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string? Description { get; set; }

        public int? TransactionType {get; set; }
    }
}
