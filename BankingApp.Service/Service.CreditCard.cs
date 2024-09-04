using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using BankingApp.Entity.Entities;
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
            EParameter eParameter = new EParameter();

            DTOCreditCard dtoCard = requestMessage.Get<DTOCreditCard>();

            List<DTOCreditCard> cardList = Mapper.Map<List<DTOCreditCard>>(await eCreditCard.GetAll()).Where(x=> x.Active == true).ToList();

            foreach(DTOCreditCard cc in cardList){
                cc.TypeName = Mapper.Map<DTOParameter>(await eParameter.GetParameter(Mapper.Map<Parameter>(new DTOParameter{GroupCode="CardType", Code = cc.Type}))).Description;
            }
            if (dtoCard.CustomerNo != null)
            {
                cardList = cardList.Where(x => x.CustomerNo.Equals(dtoCard.CustomerNo)).ToList();
            }

            response.Add(cardList);
            return response;
        }
    }
}
