using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOExpense : BaseDTO
    {
        [StringLength(16)]
        public string? CreditCardNo { get; set; }

        [StringLength(16)]
        public string? AccountNo { get; set; }

        public int? InstallmentCount { get; set; }

        public int? TransactionType { get; set; }

        public decimal? Amount { get; set; }
    }
}
