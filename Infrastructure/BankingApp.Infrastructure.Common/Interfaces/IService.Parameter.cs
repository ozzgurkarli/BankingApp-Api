using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Infrastructure.Common.Interfaces
{
    public partial interface ISInfrastructure
    {
        public Task<MessageContainer> GetParametersByGroupCode(MessageContainer requestMessage);


        public Task<MessageContainer> GetMultipleGroupCode(MessageContainer requestMessage);

        public Task<MessageContainer> SetCurrencyValuesSchedule(MessageContainer requestMessage);
        
        public Task<MessageContainer> ScheduleManager(MessageContainer requestMessage);
    }
}
