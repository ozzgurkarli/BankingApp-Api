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
            requestMessage.UnitOfWork = unitOfWork;

            MessageContainer responseCC = await proxy.GetCreditCardsByFilter(requestMessage);

            MessageContainer response = new MessageContainer();

            response.Add("CCList", responseCC.Get<List<DTOCreditCard>>());
            unitOfWork.Commit();

            return Ok(response);
        }


        [HttpPost("CardExpensePayment")]
        public async Task<IActionResult> CardExpensePayment(MessageContainer requestMessage){
            requestMessage.UnitOfWork = unitOfWork;

            MessageContainer responseCC = await proxy.CardExpensePayment(requestMessage);

            MessageContainer response = new MessageContainer();

            response.Add("CreditCard", responseCC.Get<List<DTOCreditCard>>());
            unitOfWork.Commit();

            return Ok(response);
        }

        [HttpPost("CardApplication")]
        public async Task<IActionResult> CardApplication(MessageContainer requestMessage){
            requestMessage.UnitOfWork = unitOfWork;

            MessageContainer responseCC = await proxy.NewCardApplication(requestMessage);

            MessageContainer response = new MessageContainer();

            response.Add("CreditCard", responseCC.Get<List<DTOCreditCard>>());
            unitOfWork.Commit();

            return Ok(response);
        }
        

        [HttpPost("SelectCreditCardWithDetails")]
        public async Task<IActionResult> SelectCreditCardWithDetails(MessageContainer requestMessage){
            requestMessage.UnitOfWork = unitOfWork;

            MessageContainer responseCC = await proxy.SelectCreditCardWithDetails(requestMessage);

            MessageContainer response = new MessageContainer();

            response.Add("CreditCard", responseCC.Get<DTOCreditCard>());
            unitOfWork.Commit();

            return Ok(response);
        }
    }
}