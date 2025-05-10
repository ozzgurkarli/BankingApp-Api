using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;

namespace BankingApp.Customer.Common.DataTransferObjects
{
    public class DTOMailAddresses : BaseDTO
    {
        [StringLength(12)]
        public string? CustomerNo { get; set; }

        public string? MailAddress { get; set; }

        public bool? Primary { get; set; }
    }
}
