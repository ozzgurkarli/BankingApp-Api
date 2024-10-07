using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOCustomer : BaseDTO
    {
        [Range(100000000000, 999999999999)]
        public new Int64? Id { get; set; }

        [StringLength(12)]
        public string? CustomerNo { get; set; }

        [StringLength(11)]
        public  string? IdentityNo { get; set; }

        public  string? Name { get; set; }

        public  string? Surname { get; set; }

        public int? Gender { get; set; }

        public bool? Active { get; set; }

        public decimal? Salary { get; set; }

        public string? PhoneNo { get; set; }

        public int? CreditScore { get; set; }

        public string? PrimaryMailAddress { get; set; }

        public string? AccountNo { get; set; }

        public int? Profession { get; set; }

        public int? Branch { get; set; }
    }
}
