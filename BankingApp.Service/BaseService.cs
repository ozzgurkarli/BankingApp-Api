using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Entity.Entities;

namespace BankingApp.Service
{
    public class BaseService
    {
        public IMapper Mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region customer
            CreateMap<DTOCustomer, Customer>().AfterMap((src, dest) =>
            {
                dest.Id = Convert.ToInt64(src.CustomerNo);
            });

            CreateMap<Customer, DTOCustomer>().AfterMap((src, dest) =>
            {
                dest.CustomerNo = src.Id.ToString();

                if(src.MailAddresses != null && src.MailAddresses.FirstOrDefault(x=> x.Primary) != null)
                {
                    dest.PrimaryMailAddress = src.MailAddresses.FirstOrDefault(x => x.Primary)!.MailAddress;
                }
            });
            #endregion customer

            #region login
            CreateMap<DTOLogin, Login>();
            CreateMap<Login, DTOLogin>();
            #endregion login

            #region parameter
            CreateMap<DTOParameter, Parameter>();
            CreateMap<Parameter, DTOParameter>();
            #endregion parameter

            #region account
            CreateMap<DTOAccount, Account>().AfterMap((src, dest) =>
            {
                dest.AccountNo ??= string.Empty;

                if (dest.Customer != null)
                {
                    dest.Customer.Id = Convert.ToInt64(src.CustomerNo);
                }
                else
                {
                    dest.Customer = new Customer { Id = Convert.ToInt64(src.CustomerNo) };
                }
            });
            CreateMap<Account, DTOAccount>().AfterMap((src, dest) =>
            {
                if(src.Customer != null){
                    dest.CustomerNo = src.Customer.Id.ToString();
                    dest.CustomerActive = src.Customer.Active;
                }
            });
            #endregion account

            #region transactionhistory
            CreateMap<DTOTransactionHistory, TransactionHistory>().AfterMap((src, dest) =>
            {
                if(!string.IsNullOrWhiteSpace(src.CustomerNo)){
                    dest.Customer = new Customer{Id = Int64.Parse(src.CustomerNo)};
                }

                if(!string.IsNullOrWhiteSpace(src.AccountNo)){
                    dest.Account = new Account{AccountNo = src.AccountNo};
                }

                if(!string.IsNullOrWhiteSpace(src.CreditCardNo)){
                    dest.CreditCard = new CreditCard{CardNo = src.CreditCardNo};
                }
            });
            CreateMap<TransactionHistory, DTOTransactionHistory>().AfterMap((src, dest) =>
            {
                if(src.Customer != null){
                    dest.CustomerNo = src.Customer.Id.ToString();
                }

                if(src.Account != null){
                    dest.AccountNo = src.Account.AccountNo;
                }

                if(src.CreditCard != null){
                    dest.CreditCardNo = src.CreditCard.CardNo;
                }
            });
            #endregion transactionhistory

            #region transfer
            CreateMap<DTOTransfer, Transfer>().AfterMap((src, dest) =>
            {
                if(src.SenderAccount != null){
                    dest.SenderAccount = new Account{AccountNo = src.SenderAccount};
                }
                if(src.RecipientAccount != null){
                    dest.RecipientAccount = new Account{AccountNo = src.RecipientAccount};
                }
                
            });
            CreateMap<Transfer, DTOTransfer>().AfterMap((src, dest) =>
            {
                if(src.SenderAccount != null){
                    dest.SenderAccount = src.SenderAccount.AccountNo;
                }
                if(src.RecipientAccount != null){
                    dest.RecipientAccount = src.RecipientAccount.AccountNo;
                }
            });
            #endregion transfer

            #region creditcard
            CreateMap<DTOCreditCard, CreditCard>().AfterMap((src, dest) =>
            {
                dest.CardNo ??= string.Empty;

                if (dest.Customer != null)
                {
                    dest.Customer.Id = Convert.ToInt64(src.CustomerNo);
                }
                else
                {
                    dest.Customer = new Customer { Id = Convert.ToInt64(src.CustomerNo) };
                }
            });
            CreateMap<CreditCard, DTOCreditCard>().AfterMap((src, dest) =>
            {
                if(src.Customer != null){
                    dest.CustomerNo = src.Customer.Id.ToString();
                }
            });
            #endregion creditcard

            #region mailaddresses
            CreateMap<DTOMailAddresses, MailAddresses>()
                .AfterMap((src, dest) =>
                {
                    if (dest.Customer != null)
                    {
                        dest.Customer.Id = Convert.ToInt64(src.CustomerNo);
                    }
                    else
                    {
                        dest.Customer = new Customer { Id = Convert.ToInt64(src.CustomerNo) };
                    }
                });
            CreateMap<MailAddresses, DTOMailAddresses>().AfterMap((src, dest) =>
            {
                if(src.Customer != null)
                {
                    dest.CustomerNo = src.Customer.Id.ToString();
                }
            });
            #endregion mailaddresses
        }

    }
}
