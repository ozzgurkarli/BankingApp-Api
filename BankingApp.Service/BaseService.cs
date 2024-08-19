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
            CreateMap<DTOCustomer, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerNo));

            CreateMap<Customer, DTOCustomer>()
                .ForMember(dest => dest.CustomerNo, opt => opt.MapFrom(src => src.Id));
            #endregion customer

            #region login
            CreateMap<DTOLogin, Login>();
            CreateMap<Login, DTOLogin>();
            #endregion login

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
            CreateMap<MailAddresses, DTOMailAddresses>();
            #endregion mailaddresses
        }
    }
}
