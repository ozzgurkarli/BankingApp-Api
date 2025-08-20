using BankingApp.Common.DataTransferObjects;
using BankingApp.Account.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Account.Common.DataTransferObjects;
using BankingApp.Account.Common.Interfaces;
using BankingApp.Credit.Common.Constants;
using BankingApp.Credit.Common.DataTransferObjects;
using BankingApp.Credit.Common.Interfaces;
using BankingApp.Credit.Entity;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Credit.Service
{
    public partial class SCredit: ISCredit
    {
        public async Task<MessageContainer> CreateInstallmentTransaction(MessageContainer requestMessage)
        {
            MessageContainer responseMessage = new MessageContainer();
            EInstallment eInstallment = new EInstallment(requestMessage.UnitOfWork);
            DTOCreditCard dtoCreditCard = requestMessage.Get<DTOCreditCard>();

            List<DTOInstallment> installmentList = new List<DTOInstallment>();

            decimal installmentAmount = Math.Round((decimal)(dtoCreditCard.Amount! / dtoCreditCard.InstallmentCount!),
                2, MidpointRounding.AwayFromZero);

            requestMessage.Clear();
            requestMessage.Add(new DTOTransactionHistory
            {
                Amount = dtoCreditCard.Amount, Currency = "INF", TransactionDate = DateTime.Today,
                CreditCardNo = dtoCreditCard.CardNo, CustomerNo = dtoCreditCard.CustomerNo,
                TransactionType = (int)TransactionType.Installment,
                Description = $"{dtoCreditCard.TransactionCompany}||{dtoCreditCard.InstallmentCount}"
            });

            MessageContainer responseTransaction;
            using (ISAccount proxy = _serviceProvider.GetRequiredService<ISAccount>())
            {
                responseTransaction = await proxy.AddNewTransaction(requestMessage);
            }
            DTOTransactionHistory dtoTransactionHistory =
                responseTransaction.Get<DTOTransactionHistory>()!;

            decimal overPrice = (decimal)(installmentAmount * dtoCreditCard.InstallmentCount!) -
                                (decimal)dtoCreditCard.Amount!;

            for (int i = 0; i < dtoCreditCard.InstallmentCount; i++)
            {
                if (i.Equals(dtoCreditCard.InstallmentCount - 1))
                {
                    installmentAmount -= overPrice;
                }

                installmentList.Add(new DTOInstallment
                {
                    Amount = installmentAmount, InstallmentNumber = i + 1, PaymentDate = DateTime.Today.AddMonths(i),
                    Success = false, CreditCardNo = dtoCreditCard.CardNo, TransactionId = dtoTransactionHistory.Id
                });
            }

            responseMessage.Add(await eInstallment.AddRange(installmentList));

            return responseMessage;
        }

        public async Task<MessageContainer> ExecuteInstallmentSchedule(MessageContainer requestMessage)
        {
            EInstallment eInstallment = new EInstallment(requestMessage.UnitOfWork);
            ECreditCard eCreditCard = new ECreditCard(requestMessage.UnitOfWork);
            List<DTOCreditCard> dtoCreditCardList = new List<DTOCreditCard>();
            DTOCreditCard dtoCreditCard;
            List<DTOInstallment> installmentList =
                await eInstallment.GetInstallmentsToExecute(new DTOInstallment { PaymentDate = DateTime.Today });
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
                    dtoCreditCard = dtoCreditCardList.FirstOrDefault(x => x.CardNo.Equals(item.CreditCardNo)) ??
                                    await eCreditCard.Select(new DTOCreditCard { CardNo = item.CreditCardNo });
                    dtoCreditCard.CurrentDebt += item.Amount;
                    dtoCreditCardList.Add(dtoCreditCard);
                    transactionList.Add(new DTOTransactionHistory
                    {
                        CreditCardNo = item.CreditCardNo, TransactionType = (int)TransactionType.Installment,
                        Amount = -item.Amount, Currency = CurrencyTypes.TURKISH_LIRA,
                        CustomerNo = dtoCreditCard.CustomerNo,
                        Description = $"{transactionCompany} {item.InstallmentNumber}/{installmentCount}",
                        TransactionDate = DateTime.Now
                    });
                    item.Success = true;
                }
                catch (Exception)
                {
                    //errorlog
                }
            }

            requestMessage.Clear();
            requestMessage.Add(transactionList);

            using (ISAccount proxy = _serviceProvider.GetRequiredService<ISAccount>())
            {
                await proxy.AddMultipleTransactions(requestMessage);
            }

            await eCreditCard.UpdateRange(dtoCreditCardList);
            await eInstallment.UpdateRange(installmentList);

            return new MessageContainer();
        }
    }
}