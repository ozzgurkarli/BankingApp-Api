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

        public async Task<MessageContainer> GetCreditCardsByFilter(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            ECreditCard eCreditCard = new ECreditCard();

            DTOCreditCard dtoCard = requestMessage.Get<DTOCreditCard>();

            List<DTOCreditCard> cardList = Mapper.Map<List<DTOCreditCard>>(await eCreditCard.GetAll()).Where(x=> x.Active == true).ToList();

            if (dtoCard.CustomerNo != null)
            {
                cardList = cardList.Where(x => x.CustomerNo.Equals(dtoCard.CustomerNo)).ToList();
            }

            response.Add(cardList);
            return response;
        }
    }
}
