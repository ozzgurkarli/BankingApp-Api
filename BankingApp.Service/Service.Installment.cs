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
            MessageContainer responseMessage = new MessageContainer();
            EInstallment eInstallment = new EInstallment();
            DTOExpense dtoExpense = requestMessage.Get<DTOExpense>();

            List<DTOInstallment> installmentList = new List<DTOInstallment>();
            
            decimal installmentAmount = (decimal)(dtoExpense.Amount! / dtoExpense.InstallmentCount!);
            
            for (int i = 0; i < dtoExpense.InstallmentCount; i++)
            {
                installmentList.Add(new DTOInstallment{Amount = installmentAmount, InstallmentNumber = i, PaymentDate = DateTime.Today.AddMonths(i), Success = false, CreditCardNo = dtoExpense.CreditCardNo});
            }

            responseMessage.Add(await eInstallment.AddRange(installmentList));

            return responseMessage;
        }

        public async Task<MessageContainer> ExecuteInstallmentSchedule(MessageContainer requestMessage){
            return new MessageContainer();
        }
    }
}
