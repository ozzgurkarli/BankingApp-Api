﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity.Entities
{
    public class AccountTracker: BaseEntity
    {
        public string? Currency { get; set; }

        [StringLength(16)]
        public string? FirstAvailableNo { get; set; }

        public new DateTime RecordDate { get; set; } = DateTime.MinValue;
    }
}
