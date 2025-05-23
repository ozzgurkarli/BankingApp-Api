﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Common.DataTransferObjects
{
    public class DTOParameter : BaseDTO
    {
        public string? GroupCode { get; set; }

        public int? Code { get; set; }

        public string? Description { get; set; }

        public string? Detail1 { get; set; }

        public string? Detail2 { get; set; }

        public string? Detail3 { get; set; }

        public string? Detail4 { get; set; }

        public string? Detail5 { get; set; }
    }
}
