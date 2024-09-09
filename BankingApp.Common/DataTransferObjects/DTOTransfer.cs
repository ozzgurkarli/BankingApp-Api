using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOTransfer: BaseDTO
    {
        
        public string? SenderAccount { get; set; }

        public string? RecipientAccount { get; set; }

        public string? SenderCustomerNo { get; set; }


        public int? SenderAccountId { get; set; }

        public int? RecipientAccountId { get; set; }


        public string? RecipientCustomerNo { get; set; }

        public decimal? Amount { get; set; }

        public int? Status { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? Currency { get; set; }
    }
}