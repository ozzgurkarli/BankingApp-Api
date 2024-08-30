using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOCredit: BaseDTO
    {
        [StringLength(12)]
        public required string CustomerNo { get; set; }

        public decimal? Principal { get; set; }

        public decimal? PayBack { get; set; }

        public float? MonthlyInterest { get; set; }

        public int? Maturity { get; set; }

        public bool? Paid { get; set; }

        public bool? Approved { get; set; }

        public DateTime? FirstPaymentDate { get; set; }
    }
}
