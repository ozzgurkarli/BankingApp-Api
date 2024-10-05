using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOLogin : BaseDTO
    {
        [StringLength(11)]
        public required string IdentityNo { get; set; }

        public string? Password { get; set; }

        public bool? Temporary { get; set; }

        public string? Token { get; set; }

        public DateTime? TokenExpireDate { get; set; }
    }
}
