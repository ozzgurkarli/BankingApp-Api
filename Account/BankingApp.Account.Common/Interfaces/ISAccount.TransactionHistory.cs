using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;

namespace BankingApp.Account.Common.Interfaces
{
    public partial interface ISAccount
    {
        public Task<MessageContainer> AddNewTransaction(MessageContainer requestMessage);
        public Task<MessageContainer> AddMultipleTransactions(MessageContainer requestMessage);
        public Task<MessageContainer> GetHistoryByFilter(MessageContainer requestMessage);

    }
}
