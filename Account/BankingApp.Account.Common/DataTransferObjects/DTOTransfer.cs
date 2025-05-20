using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Account.Common.DataTransferObjects
{
    public class DTOTransfer: BaseDTO
    {
        
        public string? SenderAccountNo { get; set; }

        public string? RecipientAccountNo { get; set; }
        
        public decimal? SenderAccountBalance { get; set; }

        public bool? SenderAccountActive { get; set; }

        public bool? RecipientAccountActive { get; set; }

        public bool? SenderCustomerActive { get; set; }

        public bool? RecipientCustomerActive { get; set; }

        public string? SenderMailAddress { get; set; }

        public string? RecipientMailAddress { get; set; }

        public string? SenderName { get; set; }

        public string? RecipientName { get; set; }


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