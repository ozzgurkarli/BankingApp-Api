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
            DTOCreditCard dtoCreditCard = requestMessage.Get<DTOCreditCard>();

            List<DTOInstallment> installmentList = new List<DTOInstallment>();
            
            decimal installmentAmount = Math.Round((decimal)(dtoCreditCard.Amount! / dtoCreditCard.InstallmentCount!), 2, MidpointRounding.AwayFromZero);
            
            for (int i = 0; i < dtoCreditCard.InstallmentCount; i++)
            {
                installmentList.Add(new DTOInstallment{Amount = installmentAmount, InstallmentNumber = i+1, PaymentDate = DateTime.Today.AddMonths(i), Success = false, CreditCardNo = dtoCreditCard.CardNo});
            }

            responseMessage.Add(await eInstallment.AddRange(installmentList));

            return responseMessage;
        }

        public async Task<MessageContainer> ExecuteInstallmentSchedule(MessageContainer requestMessage){
            EInstallment eInstallment = new EInstallment();

            List<DTOInstallment> installmentList = await eInstallment.GetInstallmentsToExecute(new DTOInstallment{PaymentDate = DateTime.Today});
            List<DTOTransactionHistory> transactionList = new List<DTOTransactionHistory>();
            List<Task> taskList = new List<Task>();
            foreach (var item in installmentList)
            {
                
            }

            return new MessageContainer();
        }
    }
}
