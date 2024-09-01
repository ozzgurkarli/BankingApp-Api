using BankingApp.Common.Constants;
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
    public partial class Service: BaseService, IService
    {
        public async Task<MessageContainer> CreateCustomer(MessageContainer requestMessage)
        {
            ECustomer eCustomer = new ECustomer();
            ELogin eLogin = new ELogin();
            DTOCustomer dtoReqCustomer = requestMessage.Get<DTOCustomer>();
            MessageContainer reqService = new MessageContainer();

            dtoReqCustomer.CreditScore = 1000;
            dtoReqCustomer.Active = true;

            DTOCustomer dtoCustomer = Mapper.Map<DTOCustomer>(await eCustomer.Add(Mapper.Map<Customer>(dtoReqCustomer)));
            DTOLogin dtoLogin = new DTOLogin { IdentityNo = dtoCustomer.IdentityNo, Password = setTemporaryPassword(), Temporary = true };
            eLogin.Add(Mapper.Map<Login>(dtoLogin));

            DTOAccount dtoAccount = new DTOAccount { Active = true, Branch = dtoReqCustomer.Branch, Currency = CurrencyTypes.TURKISH_LIRA, CustomerNo = dtoCustomer.CustomerNo};
            reqService.Add(dtoAccount);
            CreateAccount(reqService);

            DTOMailAddresses dtoMailAddress = new DTOMailAddresses { CustomerNo = dtoCustomer.CustomerNo!, MailAddress = dtoReqCustomer.PrimaryMailAddress!, Primary = true };

            reqService.Clear();
            reqService.Add("MailAddress", dtoMailAddress);
            reqService.Add("Customer", dtoCustomer);
            reqService.Add("Login", dtoLogin);

            AddMailAddressWithTemporaryPassword(reqService);

            reqService.Clear();
            dtoCustomer.PrimaryMailAddress = dtoMailAddress.MailAddress;
            reqService.Add(dtoCustomer);

            return reqService;
        }

        public async Task<MessageContainer> GetCustomerByIdentityNo(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            ECustomer eCustomer = new ECustomer();
            DTOCustomer dtoCustomer = requestMessage.Get<DTOCustomer>();

            dtoCustomer = Mapper.Map<DTOCustomer>(await eCustomer.GetByIdentityNo(Mapper.Map<Customer>(dtoCustomer)));

            response.Add("DTOCustomer", dtoCustomer);

            return response;
        }
    }
}
