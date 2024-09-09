using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;

namespace BankingApp.Common.Interfaces
{
    public partial interface IService
    {
        
        public Task<MessageContainer> StartTransfer(MessageContainer requestMessage);

        public Task<MessageContainer> ExecuteTransferSchedule(MessageContainer requestMessage);
    }
}