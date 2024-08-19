using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class Credit: BaseEntity
    {
        public virtual required Customer Customer { get; set; }

        public decimal Principal { get; set; }

        public decimal PayBack { get; set; }

        public float MonthlyInterest { get; set; }

        public int Maturity { get; set; }

        public bool Paid { get; set; }

        public bool Approved { get; set; }

        public DateTime? FirstPaymentDate { get; set; }
    }
}
