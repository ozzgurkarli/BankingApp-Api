using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.enums;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Service
{
    public partial class Service : BaseService, IService
    {
        public async Task<MessageContainer> CreateCustomer(MessageContainer requestMessage)
        {
            ECustomer eCustomer = new ECustomer(requestMessage.UnitOfWork);
            DTOCustomer dtoReqCustomer = requestMessage.Get<DTOCustomer>();
            MessageContainer reqService = new MessageContainer(requestMessage.UnitOfWork);

            DTOCustomer dtoCustomer = await eCustomer.Add(dtoReqCustomer);
            string tempPassword = setTemporaryPassword();
            DTOLogin dtoLogin = new DTOLogin
                { IdentityNo = dtoCustomer.IdentityNo, Password = tempPassword, Temporary = true };
            reqService.Add("Login", dtoLogin);
            RegisterCustomer(reqService);

            reqService.Clear();
            DTOAccount dtoAccount = new DTOAccount
            {
                Active = true, Primary = true, Branch = dtoReqCustomer.Branch, Currency = "TL", CurrencyCode = "1",
                CustomerNo = dtoCustomer.CustomerNo
            };
            reqService.Add(dtoAccount);
            CreateAccount(reqService);

            DTOMailAddresses dtoMailAddress = new DTOMailAddresses
            {
                CustomerNo = dtoCustomer.CustomerNo!, MailAddress = dtoReqCustomer.PrimaryMailAddress!, Primary = true
            };

            reqService.Clear();
            reqService.Add("MailAddress", dtoMailAddress);
            reqService.Add("Customer", dtoCustomer);
            dtoLogin.Password = tempPassword;
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

            dtoCustomer = await eCustomer.Get(dtoCustomer);

            response.Add("DTOCustomer", dtoCustomer);

            return response;
        }
    }
}