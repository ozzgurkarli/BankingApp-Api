﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Customer.Common.DataTransferObjects
{
    public class DTOLogin : BaseDTO
    {
        [StringLength(11)]
        public string? IdentityNo { get; set; }

        public string? Password { get; set; }

        public bool? Temporary { get; set; }

        public string? Token { get; set; }

        public string? CustomerNo { get; set; }

        public DateTime? TokenExpireDate { get; set; }

        public string? NotificationToken { get; set; }
    }
}
