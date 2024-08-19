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
        [StringLength(12)]
        public string? CustomerNo { get; set; }

        [StringLength(11)]
        public required string IdentityNo { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public bool Gender { get; set; }

        public bool Active { get; set; }

        public string? PhoneNo { get; set; }

        public int CreditScore { get; set; }

        public string? PrimaryMailAddress { get; set; }

        public int Profession { get; set; }
    }
}
