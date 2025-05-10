using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;

namespace BankingApp.Credit.Common.DataTransferObjects
{
    public class DTOCreditCard : BaseDTO
    {
        [StringLength(12)]
        public string? CustomerNo { get; set; }

        [StringLength(16)]
        public string? CardNo { get; set; }

        public decimal? Limit { get; set; }

        public decimal? CurrentDebt { get; set; }

        public decimal? TotalDebt { get; set; }

        public decimal? EndOfCycleDebt { get; set; }

        public bool? Active { get; set; }

        public Int16? CVV { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public Int16? BillingDay { get; set; }

        public int? Type { get; set; }

        public string? TypeName { get; set; }

        
        public decimal? TypeFee { get; set; }

        public decimal? FutureInstallments { get; set; }

        public DateTime? AccountClosingDate { get; set; }

        public DateTime? NextAccountClosingDate { get; set; }

        public decimal? OutstandingBalance { get; set; }

        public int? InstallmentCount { get; set; }

        public decimal? Amount { get; set; }

        public string? TransactionCompany { get; set; }
    }
}
