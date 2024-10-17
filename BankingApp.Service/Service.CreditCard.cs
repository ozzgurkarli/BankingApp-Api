using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Service
{
    public partial class Service: IService
    {
        public async Task<MessageContainer> CardRevenuePayment(MessageContainer requestMessage){
            ECreditCard eCreditCard = new ECreditCard();
            EParameter eParameter = new EParameter();

            List<DTOCreditCard> ccList = await eCreditCard.Get(new DTOCreditCard{ExpirationDate = DateTime.Today.AddYears(-1)});
            List<DTOParameter> parList = await eParameter.GetByMultipleGroupCode(new DTOParameter{GroupCode = "CardType"});

            decimal cardFee;
            for(int i = 0; i < ccList.Count; i++)
            {
                cardFee = decimal.Parse(parList.Find(x=> x.Code.Equals(ccList[i].Type))!.Detail1!);
                ccList[i] = cardExpense(ccList[i], cardFee);
            }
            
            return new MessageContainer();
        }

        public async Task<MessageContainer> CardExpensePayment(MessageContainer requestMessage){
            
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

        public async Task<MessageContainer> NewCardApplication(MessageContainer requestMessage){
            ECreditCard eCreditCard = new ECreditCard();
            EAccount eAccount = new EAccount();
            DTOCreditCard dtoCreditCard = requestMessage.Get<DTOCreditCard>();
            
            Random rnd = new Random();
            DateTime cvvDate = DateTime.Now.AddMonths(50);

            dtoCreditCard.Active = true;
            dtoCreditCard.ExpirationDate = DateTime.Now.AddMonths(50);
            dtoCreditCard.CVV = Int16.Parse(rnd.Next(100,1000).ToString());
            dtoCreditCard.OutstandingBalance = dtoCreditCard.Limit;

            dtoCreditCard.CardNo = "530129";
            string firstAvailableNo = (await eAccount.GetFirstAvailableNoAndIncrease(new DTOAccount{Currency = "CC"})).AccountNo!;

            for (int i = 0; i < 10 - int.Parse(firstAvailableNo); i++)
            {
                dtoCreditCard.CardNo += "0";
            }
            dtoCreditCard.CardNo += firstAvailableNo;

            await eCreditCard.Add(dtoCreditCard);

            return new MessageContainer();
        }

        private DTOCreditCard cardExpense(DTOCreditCard cc, decimal fee){
            cc.CurrentDebt += fee;
            cc.OutstandingBalance -= fee;
            cc.TotalDebt += fee;

            return cc;
        }
    }
}
