using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;

namespace BankingApp.Common.Constants
{
    public static class CommonMethods
    {
        public static DTOCreditCard CardExpense(DTOCreditCard cc, decimal fee, bool installment){
            if(!installment){
                cc.CurrentDebt += fee;
            }
            cc.OutstandingBalance -= fee;
            cc.TotalDebt += fee;

            return cc;
        }

        public static List<int> GetBillableDays()
        {
            List<int> billingDays = new List<int>();
            
            DateTime date = DateTime.Today.AddDays(-1);
            billingDays.Add(date.Day);
            if (DateTime.Today.Day.Equals(1))      // if months last day, get extra days
            {
                int day = date.Day;
                while (day < 31) // max day 31
                {
                    billingDays.Add(++day);
                }
            }

            return billingDays;
        }
    }
}
