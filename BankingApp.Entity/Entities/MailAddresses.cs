using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class MailAddresses: BaseEntity
    {
        public long CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public required string MailAddress { get; set; }

        public bool Primary { get; set; }
    }
}
