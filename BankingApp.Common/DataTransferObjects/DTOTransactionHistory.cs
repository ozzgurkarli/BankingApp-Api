using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOTransactionHistory: BaseDTO
    {
        public string? AccountNo { get; set; }

        public string? CreditCardNo { get; set; }

        public decimal Amount { get; set; }

        public required string Currency { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? Description { get; set; }
    }
}
