using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;

namespace BankingApp.Common.Interfaces
{
    public partial interface IService
    {
        public Task<MessageContainer> CreateInstallmentTransaction(MessageContainer requestMessage);

        public Task<MessageContainer> ExecuteInstallmentSchedule(MessageContainer requestMessage);
    }
}
