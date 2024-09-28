using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class Account: BaseEntity
    {
        public long CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [StringLength(16)]
        public string? AccountNo { get; set; }

        public decimal? Balance { get; set; }

        public string? Currency { get; set; }

        public bool? Active { get; set; }

        public bool? Primary { get; set; }

        public int? Branch { get; set; }

        public virtual List<TransactionHistory>? TransactionHistory { get; set; }
    }
}
