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
            ECreditCard eCreditCard = new ECreditCard();
            EParameter eParameter = new EParameter();

            List<DTOCreditCard> ccList = await eCreditCard.Get(new DTOCreditCard { ExpirationDate = DateTime.Today.AddYears(-1) });
            List<DTOParameter> parList = await eParameter.GetByMultipleGroupCode(new DTOParameter { GroupCode = "'CardType'" });

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
            ECreditCard eCreditCard = new ECreditCard();
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            DTOCreditCard cc = requestMessage.Get<DTOCreditCard>();

            DTOCreditCard dtoCreditCard = await eCreditCard.Select(cc);

            if (cc.Amount > dtoCreditCard.OutstandingBalance)
            {
                throw new Exception("Gerçekleştirmek istediğiniz işlem için limitiniz yetersizdir.");
            }
            else if (DateTime.Today > cc.ExpirationDate)
            {
                cc.Active = false;
                await eCreditCard.Update(cc);
                throw new Exception("Kartınızın kullanım tarihi geçmiştir, lütfen şubenize başvurun.");
            }
            else if((bool)!cc.Active!)
            {
                throw new Exception("Kartınız kullanıma kapalıdır.");
            }
            
            dtoCreditCard.Amount = cc.Amount;
            dtoCreditCard.TransactionCompany = cc.TransactionCompany;

            if (cc.InstallmentCount != null && cc.InstallmentCount > 0)
            {
                MessageContainer requestInstallment = new MessageContainer();
                dtoCreditCard.InstallmentCount = cc.InstallmentCount;
                requestInstallment.Add(dtoCreditCard);
                MessageContainer responseInstallment = await CreateInstallmentTransaction(requestInstallment);
                CommonMethods.CardExpense(dtoCreditCard, (decimal)cc.Amount!, true);
            }
            else
            {
                DTOTransactionHistory dtoTH = new DTOTransactionHistory { CustomerNo = dtoCreditCard.CustomerNo, Amount = -dtoCreditCard.Amount, CreditCardNo = dtoCreditCard.CardNo, Currency = CurrencyTypes.TURKISH_LIRA, TransactionDate = DateTime.Today, TransactionType = (int?)TransactionType.Expense };
                MessageContainer requestTH = new MessageContainer();
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
            ECreditCard eCreditCard = new ECreditCard();
            EParameter eParameter = new EParameter();

            DTOCreditCard dtoCard = requestMessage.Get<DTOCreditCard>();

            List<DTOCreditCard> cardList = await eCreditCard.Get(dtoCard);

            response.Add(cardList);
            return response;
        }

        public async Task<MessageContainer> NewCardApplication(MessageContainer requestMessage)
        {
            ECreditCard eCreditCard = new ECreditCard();
            EAccount eAccount = new EAccount();
            DTOCreditCard dtoCreditCard = requestMessage.Get<DTOCreditCard>();

            Random rnd = new Random();
            DateTime cvvDate = DateTime.Now.AddMonths(50);

            dtoCreditCard.Active = true;
            dtoCreditCard.ExpirationDate = DateTime.Now.AddMonths(50);
            dtoCreditCard.CVV = Int16.Parse(rnd.Next(100, 1000).ToString());
            dtoCreditCard.OutstandingBalance = dtoCreditCard.Limit;

            dtoCreditCard.CardNo = "530129";
            string firstAvailableNo = (await eAccount.GetFirstAvailableNoAndIncrease(new DTOAccount { Currency = "CC" })).AccountNo!;

            for (int i = 0; i < 10 - int.Parse(firstAvailableNo); i++)
            {
                dtoCreditCard.CardNo += "0";
            }
            dtoCreditCard.CardNo += firstAvailableNo;

            await eCreditCard.Add(dtoCreditCard);

            return new MessageContainer();
        }

    }
}
