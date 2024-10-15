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
    public partial class Service: BaseService, IService
    {
        public async Task<MessageContainer> CreateInstallmentTransaction(MessageContainer requestMessage){
            EInstallment eInstallment = new EInstallment();

            return new MessageContainer();
        }
    }
}
