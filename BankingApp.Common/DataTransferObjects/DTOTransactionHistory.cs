using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOTransactionHistory: BaseDTO
    {
        public string? CustomerNo { get; set; }

        public string? AccountNo { get; set; }

        public int? AccountId { get; set; }

        public string? CreditCardNo { get; set; }

        public decimal? Amount { get; set; }

        public required string Currency { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }

        public string? Description { get; set; }

        public int? TransactionType {get; set; }

        public int? Count {get; set; }
    }
}
