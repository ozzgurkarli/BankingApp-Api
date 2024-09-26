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
    public class CreditCardController : ControllerBase
    {
        private readonly IService _proxy;

        public CreditCardController(IService proxy)
        {
            _proxy = proxy;
        }

        [HttpPost("GetCreditCards")]
        public async Task<IActionResult> GetCreditCards(MessageContainer requestMessage){
            DTOCreditCard dtoCreditCard = requestMessage.ToObject<DTOCreditCard>(requestMessage, "DTOCreditCard");

            MessageContainer requestCCard = new MessageContainer();

            requestCCard.Add(dtoCreditCard);

            MessageContainer responseCC = await _proxy.GetCreditCardsByFilter(requestCCard);

            MessageContainer response = new MessageContainer();

            response.Add("CCList", responseCC.Get<List<DTOCreditCard>>());

            return Ok(response);
        }

        [HttpPost("CardApplication")]
        public async Task<IActionResult> CardApplication(MessageContainer requestMessage){

            DTOCreditCard dtoCreditCard = requestMessage.ToObject<DTOCreditCard>(requestMessage, "DTOCreditCard");

            MessageContainer requestCCard = new MessageContainer();

            requestCCard.Add(dtoCreditCard);

            MessageContainer responseCC = await _proxy.NewCardApplication(requestCCard);

            MessageContainer response = new MessageContainer();

            response.Add("CreditCard", responseCC.Get<List<DTOCreditCard>>());

            return Ok(response);
        }
    }
}