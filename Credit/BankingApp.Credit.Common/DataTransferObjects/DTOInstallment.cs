using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Credit.Common.DataTransferObjects
{
    public class DTOInstallment : BaseDTO
    {
        [StringLength(16)]
        public string? CreditCardNo { get; set; }

        public int? InstallmentNumber { get; set; }

        public DateTime? PaymentDate { get; set; }

        public bool? Success { get; set; }

        public decimal? Amount { get; set; }

        public int? TransactionId { get; set; }

        public string? TransactionCompany { get; set; }
    }
}
