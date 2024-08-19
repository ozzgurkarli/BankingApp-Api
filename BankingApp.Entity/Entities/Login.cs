using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class Login: BaseEntity
    {
        [StringLength(11)]
        public required string IdentityNo { get; set; }

        [Range(100000, 999999)]
        public int Password { get; set; }

        public bool Temporary { get; set; }
    }
}
