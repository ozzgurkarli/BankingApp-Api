using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;

namespace BankingApp.Account.Common.Interfaces
{
    public partial interface IService
    {

        public Task<MessageContainer> CheckRecipientCustomer(MessageContainer requstMessage);

        public Task<MessageContainer> StartTransfer(MessageContainer requestMessage);

        public Task<MessageContainer> ExecuteTransferSchedule(MessageContainer requestMessage);
    }
}