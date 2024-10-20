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
    }
}
