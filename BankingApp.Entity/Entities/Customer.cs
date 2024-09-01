using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class Customer: BaseEntity
    {
        [Range(100000000000, 999999999999)]
        public new Int64 Id { get; set; }

        [StringLength(11)]
        public string? IdentityNo { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int Gender { get; set; }

        public bool Active { get; set; }

        public string? PhoneNo { get; set; }

        public decimal Salary { get; set; }

        public int CreditScore { get; set; }

        public virtual List<MailAddresses>? MailAddresses { get; set; }

        public virtual List<Account>? Accounts { get; set; }

        public virtual int Profession { get; set; }

        public virtual List<Credit>? Credits { get; set; }

        public virtual List<CreditCard>? CreditCards { get; set; }
    }
}
