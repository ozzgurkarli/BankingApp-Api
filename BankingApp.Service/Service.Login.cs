using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using BankingApp.Entity.Entities;

namespace BankingApp.Service
{
    public partial class Service: IService
    {
        public MessageContainer RegisterCustomer(MessageContainer requestMessage)
        {
            ELogin eLogin = new ELogin();
            DTOLogin dtoLogin = Mapper.Map<DTOLogin>(eLogin.Add(Mapper.Map<Login>(requestMessage.Get<DTOLogin>("Login"))));

            requestMessage.Clear();
            requestMessage.Add("Login", dtoLogin);
            return requestMessage;
        }

        private int setTemporaryPassword()
        {
            Random random = new Random();

            return random.Next(100000, 1000000);
        }
    }
}
