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
            EParameter eParameter = new EParameter();
            DTOAccount dtoAccount = requestMessage.Get<DTOAccount>();

            dtoAccount.Active = true;
            dtoAccount.Balance = 0;
            
            dtoAccount.AccountNo = (await eAccount.GetFirstAvailableNoAndIncrease(dtoAccount)).AccountNo;
            response.Add(await eAccount.Add(dtoAccount));

            return response;
        }

        public async Task<MessageContainer> GetAccountsByFilter(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            EAccount eAccount = new EAccount();

            DTOAccount dtoAccount = requestMessage.Get<DTOAccount>();
            
            List<DTOAccount> accountList = Mapper.Map<List<DTOAccount>>(await eAccount.GetAll()).Where(x => x.Active == true).OrderBy(x=> x.RecordDate).ToList();

            if(dtoAccount.CustomerNo != null)
            {
                accountList = accountList.Where(x => x.CustomerNo.Equals(dtoAccount.CustomerNo)).ToList();
            }

            response.Add(accountList);
            return response;
        }
    }
}
