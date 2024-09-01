using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class DTOParameter : BaseDTO
    {
        public required string GroupCode { get; set; }

        public int? Code { get; set; }

        public string? Description { get; set; }

        public string? Detail1 { get; set; }

        public string? Detail2 { get; set; }

        public string? Detail3 { get; set; }
    }
}
