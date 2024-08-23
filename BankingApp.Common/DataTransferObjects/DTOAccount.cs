using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOAccount : BaseDTO
    {
        [StringLength(12)]
        public string? CustomerNo { get; set; }

        [StringLength(16)]
        public string? AccountNo { get; set; }

        public decimal Balance { get; set; }

        public string? Currency { get; set; }

        public bool Active { get; set; }

        public bool Primary { get; set; }

        public string? Branch { get; set; }

        public virtual List<DTOTransactionHistory>? TransactionHistory { get; set; }
    }
}
