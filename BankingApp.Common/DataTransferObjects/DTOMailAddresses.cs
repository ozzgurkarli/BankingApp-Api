﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOMailAddresses : BaseDTO
    {
        [StringLength(12)]
        public required string CustomerNo { get; set; }

        public required string MailAddress { get; set; }

        public bool Primary { get; set; }
    }
}
