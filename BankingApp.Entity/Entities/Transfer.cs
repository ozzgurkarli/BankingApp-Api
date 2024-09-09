using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class Transfer: BaseEntity
    {
        public Account? SenderAccount { get; set; }

        public Account? RecipientAccount { get; set; }

        public decimal? Amount { get; set; }

        public int? Status { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? Currency { get; set; }
    }
}