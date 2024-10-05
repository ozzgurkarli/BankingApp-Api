using BankingApp.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.Interfaces
{
    public partial interface IService
    {
        public Task<MessageContainer> RegisterCustomer(MessageContainer requestMessage);

        public Task<MessageContainer> RegisterCheckDataAlreadyInUse(MessageContainer requestMessage);

        public Task<MessageContainer> GetLoginCredentials(MessageContainer requestMessage);

        public Task<MessageContainer> UpdatePassword(MessageContainer requestMessage);
    }
}
