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
        public Task<MessageContainer> GetParametersByGroupCode(MessageContainer requestMessage);


        public Task<MessageContainer> GetMultipleGroupCode(MessageContainer requestMessage);

        public Task<MessageContainer> SetCurrencyValues(MessageContainer requestMessage);
    }
}
