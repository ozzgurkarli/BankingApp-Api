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
        public async Task<MessageContainer> CreateInstallmentTransaction(MessageContainer requestMessage)
        {
            MessageContainer responseMessage = new MessageContainer();
            EInstallment eInstallment = new EInstallment();
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            DTOCreditCard dtoCreditCard = requestMessage.Get<DTOCreditCard>();

            List<DTOInstallment> installmentList = new List<DTOInstallment>();

            decimal installmentAmount = Math.Round((decimal)(dtoCreditCard.Amount! / dtoCreditCard.InstallmentCount!), 2, MidpointRounding.AwayFromZero);

            DTOTransactionHistory dtoTransactionHistory = await eTransactionHistory.Add(new DTOTransactionHistory { Amount = dtoCreditCard.Amount, Currency = "INF", TransactionDate = DateTime.Today, CreditCardNo = dtoCreditCard.CardNo, CustomerNo = dtoCreditCard.CustomerNo, TransactionType = (int)TransactionType.Installment, Description = $"{dtoCreditCard.TransactionCompany}||{dtoCreditCard.InstallmentCount}"});
            
            decimal overPrice = (decimal)(installmentAmount * dtoCreditCard.InstallmentCount!) - (decimal)dtoCreditCard.Amount!;
            
            for (int i = 0; i < dtoCreditCard.InstallmentCount; i++)
            {
                if (i.Equals(dtoCreditCard.InstallmentCount - 1))
                {
                    installmentAmount -= overPrice;
                }
                installmentList.Add(new DTOInstallment { Amount = installmentAmount, InstallmentNumber = i + 1, PaymentDate = DateTime.Today.AddMonths(i), Success = false, CreditCardNo = dtoCreditCard.CardNo, TransactionId = dtoTransactionHistory.Id });
            }

            responseMessage.Add(await eInstallment.AddRange(installmentList));

            return responseMessage;
        }

        public async Task<MessageContainer> ExecuteInstallmentSchedule(MessageContainer requestMessage)
        {
            EInstallment eInstallment = new EInstallment();
            ECreditCard eCreditCard = new ECreditCard();
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            List<DTOCreditCard> dtoCreditCardList = new List<DTOCreditCard>();
            DTOCreditCard dtoCreditCard;
            List<DTOInstallment> installmentList = await eInstallment.GetInstallmentsToExecute(new DTOInstallment { PaymentDate = DateTime.Today });
            List<DTOTransactionHistory> transactionList = new List<DTOTransactionHistory>();

            string transactionCompany = string.Empty;
            string installmentCount = string.Empty;
            foreach (var item in installmentList)
            {
                item.TransactionCompany ??= "UNKNOWN";
                transactionCompany = item.TransactionCompany.Substring(0, item.TransactionCompany.Length - 3);
                installmentCount = item.TransactionCompany!.Substring(item.TransactionCompany.Length - 1);
                try
                {
                    dtoCreditCard = dtoCreditCardList.FirstOrDefault(x=> x.CardNo.Equals(item.CreditCardNo)) ?? await eCreditCard.Select(new DTOCreditCard { CardNo = item.CreditCardNo });
                    dtoCreditCard.CurrentDebt += item.Amount;
                    dtoCreditCardList.Add(dtoCreditCard);
                    transactionList.Add(new DTOTransactionHistory { CreditCardNo = item.CreditCardNo, TransactionType = (int)TransactionType.Installment, Amount = -item.Amount, Currency = CurrencyTypes.TURKISH_LIRA, CustomerNo = dtoCreditCard.CustomerNo, Description = $"{transactionCompany} {item.InstallmentNumber}/{installmentCount}", TransactionDate = DateTime.Now});
                    item.Success = true;
                }
                catch (Exception)
                {
                    //errorlog
                }
            }

            await eCreditCard.UpdateRange(dtoCreditCardList);
            await eTransactionHistory.AddRange(transactionList);
            await eInstallment.UpdateRange(installmentList);

            return new MessageContainer();
        }
    }
}
