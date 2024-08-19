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
        public MessageContainer CreateCustomer(MessageContainer requestMessage)
        {
            ECustomer eCustomer = new ECustomer();
            DTOCustomer dtoReqCustomer = requestMessage.Get<DTOCustomer>();
            MessageContainer reqService = new MessageContainer();

            dtoReqCustomer.CreditScore = 1000;
            dtoReqCustomer.Active = true;

            DTOCustomer dtoCustomer = Mapper.Map<DTOCustomer>(eCustomer.Add(Mapper.Map<Customer>(dtoReqCustomer)));
            DTOLogin dtoLogin = new DTOLogin { IdentityNo = dtoCustomer.IdentityNo, Password = setTemporaryPassword() };
            reqService.Add("Login", dtoLogin);
            reqService = RegisterCustomer(reqService);

            DTOMailAddresses dtoMailAddress = new DTOMailAddresses { CustomerNo = dtoCustomer.CustomerNo, MailAddress = dtoReqCustomer.PrimaryMailAddress!, Primary = true };

            reqService.Add("MailAddress", dtoMailAddress);
            reqService.Add("Customer", dtoCustomer);

            AddMailAddressWithTemporaryPassword(reqService);

            reqService.Clear();
            reqService.Add(dtoCustomer);

            return reqService;
        }
    }
}
