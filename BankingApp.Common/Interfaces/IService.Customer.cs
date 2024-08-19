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
        public T Create<T>() where T : IService, new()
        {
            return new T();
        }

        public MessageContainer CreateCustomer(MessageContainer requestMessage);
    }
}
