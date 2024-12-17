using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CreditCardController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpPost("GetCreditCards")]
        public async Task<IActionResult> GetCreditCards(MessageContainer requestMessage){
            DTOCreditCard dtoCreditCard = requestMessage.ToObject<DTOCreditCard>(requestMessage, "DTOCreditCard");

            MessageContainer requestCCard = new MessageContainer(unitOfWork);

            requestCCard.Add(dtoCreditCard);

            MessageContainer responseCC = await proxy.GetCreditCardsByFilter(requestCCard);

            MessageContainer response = new MessageContainer();

            response.Add("CCList", responseCC.Get<List<DTOCreditCard>>());
            unitOfWork.Commit();

            return Ok(response);
        }


        [HttpPost("CardExpensePayment")]
        public async Task<IActionResult> CardExpensePayment(MessageContainer requestMessage){

            DTOCreditCard dtoCreditCard = requestMessage.ToObject<DTOCreditCard>(requestMessage, "DTOCreditCard");

            MessageContainer requestCCard = new MessageContainer(unitOfWork);

            requestCCard.Add(dtoCreditCard);

            MessageContainer responseCC = await proxy.CardExpensePayment(requestCCard);

            MessageContainer response = new MessageContainer();

            response.Add("CreditCard", responseCC.Get<List<DTOCreditCard>>());
            unitOfWork.Commit();

            return Ok(response);
        }

        [HttpPost("CardApplication")]
        public async Task<IActionResult> CardApplication(MessageContainer requestMessage){

            DTOCreditCard dtoCreditCard = requestMessage.ToObject<DTOCreditCard>(requestMessage, "DTOCreditCard");

            MessageContainer requestCCard = new MessageContainer();

            requestCCard.Add(dtoCreditCard);

            MessageContainer responseCC = await proxy.NewCardApplication(requestCCard);

            MessageContainer response = new MessageContainer();

            response.Add("CreditCard", responseCC.Get<List<DTOCreditCard>>());
            unitOfWork.Commit();

            return Ok(response);
        }
        

        [HttpPost("SelectCreditCardWithDetails")]
        public async Task<IActionResult> SelectCreditCardWithDetails(MessageContainer requestMessage){

            DTOCreditCard dtoCreditCard = requestMessage.ToObject<DTOCreditCard>(requestMessage, "DTOCreditCard");

            MessageContainer requestCCard = new MessageContainer();

            requestCCard.Add(dtoCreditCard);

            MessageContainer responseCC = await proxy.SelectCreditCardWithDetails(requestCCard);

            MessageContainer response = new MessageContainer();

            response.Add("CreditCard", responseCC.Get<DTOCreditCard>());
            unitOfWork.Commit();

            return Ok(response);
        }
    }
}