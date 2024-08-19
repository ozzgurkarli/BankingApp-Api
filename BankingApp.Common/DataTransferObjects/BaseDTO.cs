using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class BaseDTO
    {
        public int Id { get; set; }

        public DateTime RecordDate { get; set; } = DateTime.Now;

        public string RecordScreen { get; set; } = "ADMIN";
    }
}
