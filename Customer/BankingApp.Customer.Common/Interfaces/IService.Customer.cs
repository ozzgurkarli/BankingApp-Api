using BankingApp.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Customer.Common.Interfaces
{
    public partial interface ISCustomer
    {
        public Task<MessageContainer> CreateCustomer(MessageContainer requestMessage);

        public Task<MessageContainer> GetCustomerByIdentityNo(MessageContainer requestMessage);
    }
}
