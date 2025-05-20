using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Account.Common.DataTransferObjects;
using BankingApp.Account.Common.Interfaces;
using BankingApp.Customer.Common.DataTransferObjects;
using BankingApp.Customer.Common.Interfaces;
using BankingApp.Customer.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Customer.Service
{
    public partial class SCustomer(IServiceProvider _serviceProvider) : ISCustomer
    {
        
        public async Task<MessageContainer> CreateCustomer(MessageContainer requestMessage)
        {
            ECustomer eCustomer = new ECustomer(requestMessage.UnitOfWork);
            DTOCustomer dtoReqCustomer = requestMessage.Get<DTOCustomer>();
            DTOLogin dtoLogin = requestMessage.Get<DTOLogin>();
            MessageContainer reqService = new MessageContainer(requestMessage.UnitOfWork);

            DTOCustomer dtoCustomer = await eCustomer.Add(dtoReqCustomer);
            string tempPassword = setTemporaryPassword();
            dtoLogin.Password = tempPassword;
            dtoLogin.Temporary = true;
            reqService.Add("Login", dtoLogin);
            await RegisterCustomer(reqService);

            reqService.Clear();
            DTOAccount dtoAccount = new DTOAccount
            {
                Active = true, Primary = true, Branch = dtoReqCustomer.Branch, Currency = "TL", CurrencyCode = "1",
                CustomerNo = dtoCustomer.CustomerNo
            };
            reqService.Add(dtoAccount);

            using (ISAccount proxy = _serviceProvider.GetRequiredService<ISAccount>())
            {
                await proxy.CreateAccount(reqService);
            }

            DTOMailAddresses dtoMailAddress = new DTOMailAddresses
            {
                CustomerNo = dtoCustomer.CustomerNo!, MailAddress = dtoReqCustomer.PrimaryMailAddress!, Primary = true
            };

            reqService.Clear();
            reqService.Add("MailAddress", dtoMailAddress);
            reqService.Add("Customer", dtoCustomer);
            dtoLogin.Password = tempPassword;
            reqService.Add("Login", dtoLogin);

            await AddMailAddressWithTemporaryPassword(reqService);

            reqService.Clear();
            dtoCustomer.PrimaryMailAddress = dtoMailAddress.MailAddress;
            reqService.Add(dtoCustomer);

            return reqService;
        }

        public async Task<MessageContainer> GetCustomerByIdentityNo(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            ECustomer eCustomer = new ECustomer(requestMessage.UnitOfWork);
            DTOCustomer dtoCustomer = requestMessage.Get<DTOCustomer>();

            dtoCustomer = await eCustomer.Get(dtoCustomer);

            response.Add("DTOCustomer", dtoCustomer);

            return response;
        }
    }
}