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
    public class AccountController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpPost("GetAccounts")]
        public async Task<IActionResult> GetAccounts(MessageContainer requestMessage){
            DTOAccount dtoAccount = requestMessage.ToObject<DTOAccount>(requestMessage, "DTOAccount");

            MessageContainer requestAccount = new MessageContainer(unitOfWork);

            requestAccount.Add(dtoAccount);

            MessageContainer responseAcc = await proxy.GetAccountsByFilter(requestAccount);
            MessageContainer response = new MessageContainer();

            response.Add("AccountList", responseAcc.Get<List<DTOAccount>>());

            return Ok(response);
        }

        [HttpPost("AddAccount")]
        public async Task<IActionResult> AddAccount(MessageContainer requestMessage){
            DTOAccount dtoAccount = requestMessage.ToObject<DTOAccount>(requestMessage, "DTOAccount");

            MessageContainer requestAccount = new MessageContainer(unitOfWork);
            requestAccount.Add(dtoAccount);

            MessageContainer responseAcc = await proxy.CreateAccount(requestAccount);
            MessageContainer response = new MessageContainer();

            response.Add("Account", responseAcc.Get<DTOAccount>());
            unitOfWork.Commit();

            return Ok(response);
        }
    }
}