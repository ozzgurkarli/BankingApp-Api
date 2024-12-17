using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.enums;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Service
{
    public partial class Service : IService
    {
        public async Task<MessageContainer> CardRevenuePaymentSchedule(MessageContainer requestMessage)
        {
            ECreditCard eCreditCard = new ECreditCard(requestMessage.UnitOfWork);

            List<DTOCreditCard> ccList = await eCreditCard.Get(new DTOCreditCard
                { ExpirationDate = DateTime.Today.AddYears(-1) });

            MessageContainer requestParameter = new MessageContainer(requestMessage.UnitOfWork);
            requestParameter.Add(new List<DTOParameter> { new DTOParameter { GroupCode = "CardType" } });
            List<DTOParameter> parList = (await GetMultipleGroupCode(requestParameter)).Get<List<DTOParameter>>();

            decimal cardFee;
            for (int i = 0; i < ccList.Count; i++)
            {
                cardFee = decimal.Parse(parList.Find(x => x.Code.Equals(ccList[i].Type))!.Detail1!);
                ccList[i] = CommonMethods.CardExpense(ccList[i], cardFee, false);
            }

            await eCreditCard.UpdateRange(ccList);

            return new MessageContainer();
        }

        public async Task<MessageContainer> CardExpensePayment(MessageContainer requestMessage)
        {
            ECreditCard eCreditCard = new ECreditCard(requestMessage.UnitOfWork);
            DTOCreditCard cc = requestMessage.Get<DTOCreditCard>();

            DTOCreditCard dtoCreditCard = await eCreditCard.Select(cc);

            if (cc.Amount > dtoCreditCard.OutstandingBalance)
            {
                throw new Exception("Gerçekleştirmek istediğiniz işlem için limitiniz yetersizdir.");
            }
            else if (DateTime.Today > dtoCreditCard.ExpirationDate)
            {
                dtoCreditCard.Active = false;
                await eCreditCard.Update(dtoCreditCard);
                throw new Exception("Kartınızın kullanım tarihi geçmiştir, lütfen şubenize başvurun.");
            }
            else if ((bool)!dtoCreditCard.Active!)
            {
                throw new Exception("Kartınız kullanıma kapalıdır.");
            }

            dtoCreditCard.Amount = cc.Amount;
            dtoCreditCard.TransactionCompany = cc.TransactionCompany;

            if (cc.InstallmentCount != null && cc.InstallmentCount > 0)
            {
                MessageContainer requestInstallment = new MessageContainer(requestMessage.UnitOfWork);
                dtoCreditCard.InstallmentCount = cc.InstallmentCount;
                requestInstallment.Add(dtoCreditCard);
                MessageContainer responseInstallment = await CreateInstallmentTransaction(requestInstallment);
                CommonMethods.CardExpense(dtoCreditCard, (decimal)cc.Amount!, true);
            }
            else
            {
                DTOTransactionHistory dtoTH = new DTOTransactionHistory
                {
                    CustomerNo = dtoCreditCard.CustomerNo, Amount = -dtoCreditCard.Amount,
                    CreditCardNo = dtoCreditCard.CardNo, Currency = CurrencyTypes.TURKISH_LIRA,
                    TransactionDate = DateTime.Today, TransactionType = (int?)TransactionType.Expense
                };
                MessageContainer requestTH = new MessageContainer(requestMessage.UnitOfWork);
                requestTH.Add(dtoTH);
                await AddNewTransaction(requestTH);

                CommonMethods.CardExpense(dtoCreditCard, (decimal)cc.Amount!, false);
            }

            dtoCreditCard = await eCreditCard.Update(dtoCreditCard);

            return new MessageContainer();
        }

        public async Task<MessageContainer> GetCreditCardsByFilter(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            ECreditCard eCreditCard = new ECreditCard(requestMessage.UnitOfWork);

            DTOCreditCard dtoCard = requestMessage.Get<DTOCreditCard>();

            List<DTOCreditCard> cardList = await eCreditCard.Get(dtoCard);

            response.Add(cardList.OrderBy(x => x.CardNo).ToList());
            return response;
        }

        public async Task<MessageContainer> SelectCreditCardWithDetails(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            ECreditCard eCreditCard = new ECreditCard(requestMessage.UnitOfWork);

            DTOCreditCard dtoCard = requestMessage.Get<DTOCreditCard>();

            dtoCard = await eCreditCard.SelectWithDetails(dtoCard);

            DateTime date = DateTime.Today;

            if (dtoCard.BillingDay >= date.Day)
            {
                date = date.AddMonths(-1);
            }


            dtoCard.AccountClosingDate = new DateTime(year: date.Year, month: date.Month,
                day: setBillingDay(date, int.Parse(dtoCard.BillingDay.ToString()!)));

            date = date.AddMonths(1);
            dtoCard.NextAccountClosingDate = new DateTime(year: date.Year, month: date.Month,
                day: setBillingDay(date, int.Parse(dtoCard.BillingDay.ToString()!)));

            response.Add(dtoCard);

            return response;
        }

        public async Task<MessageContainer> AccountClosingSchedule(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            ECreditCard eCreditCard = new ECreditCard(requestMessage.UnitOfWork);
            List<Task<List<DTOCreditCard>>> taskList = new List<Task<List<DTOCreditCard>>>();

            DTOCreditCard dtoCreditCard = new DTOCreditCard();
            List<DTOCreditCard> updatedCcList = new List<DTOCreditCard>();

            foreach (int day in CommonMethods.GetBillableDays())
            {
                dtoCreditCard.BillingDay = short.Parse(day.ToString());
                taskList.Add(eCreditCard.Get(dtoCreditCard));
            }

            List<List<DTOCreditCard>> ccList = (await Task.WhenAll(taskList.ToArray())).ToList();

            foreach (List<DTOCreditCard> ccL in ccList)
            {
                foreach (DTOCreditCard cc in ccL)
                {
                    cc.EndOfCycleDebt += cc.CurrentDebt;
                    updatedCcList.Add(cc);
                }
            }

            response.Add(await eCreditCard.UpdateRange(updatedCcList));

            return response;
        }

        public async Task<MessageContainer> NewCardApplication(MessageContainer requestMessage)
        {
            ECreditCard eCreditCard = new ECreditCard(requestMessage.UnitOfWork);
            EAccount eAccount = new EAccount(requestMessage.UnitOfWork);
            DTOCreditCard dtoCreditCard = requestMessage.Get<DTOCreditCard>();

            Random rnd = new Random();
            DateTime cvvDate = DateTime.Now.AddMonths(50);

            dtoCreditCard.Active = true;
            dtoCreditCard.ExpirationDate = DateTime.Now.AddMonths(50);
            dtoCreditCard.CVV = Int16.Parse(rnd.Next(100, 1000).ToString());
            dtoCreditCard.OutstandingBalance = dtoCreditCard.Limit;

            dtoCreditCard.CardNo = "530129";
            string firstAvailableNo =
                (await eAccount.GetFirstAvailableNoAndIncrease(new DTOAccount { Currency = "CC" })).AccountNo!;

            for (int i = 0; i < 10 - int.Parse(firstAvailableNo); i++)
            {
                dtoCreditCard.CardNo += "0";
            }

            dtoCreditCard.CardNo += firstAvailableNo;

            await eCreditCard.Add(dtoCreditCard);

            return new MessageContainer();
        }

        private int setBillingDay(DateTime date, int billingDay)
        {
            if (billingDay > DateTime.DaysInMonth(date.Year, date.Month))
            {
                billingDay = DateTime.DaysInMonth(date.Year, date.Month);
            }

            return billingDay;
        }
    }
}