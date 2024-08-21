using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using BankingApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Service
{
    public partial class Service: IService
    {
        public async Task<MessageContainer> CreateAccount(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            EAccount eAccount = new EAccount();
            DTOAccount dtoAccount = requestMessage.Get<DTOAccount>();

            dtoAccount = Mapper.Map<DTOAccount>(await eAccount.Add(Mapper.Map<Account>(dtoAccount)));

            response.Add(dtoAccount);

            return response;
        }
    }
}
