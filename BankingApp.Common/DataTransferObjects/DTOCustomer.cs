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
        public required string CustomerNo { get; set; }

        [StringLength(11)]
        public required string IdentityNo { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public bool Gender { get; set; }

        public bool Active { get; set; }

        public string? PhoneNo { get; set; }

        public int CreditScore { get; set; }

        public List<DTOMailAddresses>? MailAddresses { get; set; }

        public required List<DTOAccount> Accounts { get; set; }

        public string? Profession { get; set; }

        public List<DTOCredit>? Credits { get; set; }

        public List<DTOCreditCard>? CreditCards { get; set; }
    }
}
