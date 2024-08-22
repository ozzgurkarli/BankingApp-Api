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
                dest.CustomerNo = src.Customer.Id.ToString();
            });
            #endregion account

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
                dest.CustomerNo = src.Customer.Id.ToString();
            });
            #endregion mailaddresses
        }

    }
}
