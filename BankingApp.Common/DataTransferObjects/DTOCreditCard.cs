using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOCreditCard : BaseDTO
    {
        [StringLength(12)]
        public required string CustomerNo { get; set; }

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
