using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class AccountTracker: BaseEntity
    {
        public required string Currency { get; set; }

        [StringLength(16)]
        public required string FirstAvailableNo { get; set; }
    }
}
