using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class Customer: BaseEntity
    {
        public required string CustomerNo { get; set; }

        public required string IdentityNo { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public bool Gender { get; set; }

        public bool Active { get; set; }

        public string? PhoneNo { get; set; }

        public virtual List<MailAddresses>? MailAddresses { get; set; }

        public virtual List<Account>? Accounts { get; set; }


    }
}
