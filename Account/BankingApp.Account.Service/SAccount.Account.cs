using BankingApp.Common.DataTransferObjects;
using BankingApp.Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Account.Common.DataTransferObjects;
using BankingApp.Account.Common.Interfaces;
using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Account.Service
{
    public partial class SAccount : ISAccount
    {
        public async Task<MessageContainer> CreateAccount(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            EAccount eAccount = new EAccount(requestMessage.UnitOfWork);
            DTOAccount dtoAccount = requestMessage.Get<DTOAccount>();
            dtoAccount.Active = true;
            dtoAccount.Balance = 0;

            dtoAccount.AccountNo = (await eAccount.GetFirstAvailableNoAndIncrease(dtoAccount)).AccountNo;
            response.Add(await eAccount.Add(dtoAccount));

            return response;
        }

        public async Task<MessageContainer> UpdateRangeAccount(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            EAccount eAccount = new EAccount(requestMessage.UnitOfWork);
            response.Add(await eAccount.UpdateRange(requestMessage.Get<List<DTOAccount>>()));

            return response;
        }

        public async Task<MessageContainer> GetAccountsByFilter(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            EAccount eAccount = new EAccount(requestMessage.UnitOfWork);

            DTOAccount dtoAccount = requestMessage.Get<DTOAccount>();

            List<DTOAccount> accountList = (await eAccount.Get(dtoAccount)).OrderBy(x => x.AccountNo).ToList();

            response.Add(accountList);
            return response;
        }
    }
}