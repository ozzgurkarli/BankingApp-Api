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
        public Task<MessageContainer> CardRevenuePaymentSchedule(MessageContainer requestMessage);
        
        public Task<MessageContainer> AccountClosingSchedule(MessageContainer requestMessage);

        public Task<MessageContainer> CardExpensePayment(MessageContainer requestMessage);

        public Task<MessageContainer> GetCreditCardsByFilter(MessageContainer requestMessage);

        public Task<MessageContainer> NewCardApplication(MessageContainer requestMessage);
    }
}
