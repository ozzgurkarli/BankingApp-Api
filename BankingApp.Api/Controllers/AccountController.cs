using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Account.Common.DataTransferObjects;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpPost("GetAccounts")]
        public async Task<IActionResult> GetAccounts(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            
            
            
            MessageContainer responseAcc = await proxy.GetAccountsByFilter(requestMessage);
            MessageContainer response = new MessageContainer();
            unitOfWork.Commit();

            response.Add("AccountList", responseAcc.Get<List<DTOAccount>>());

            return Ok(response);
        }

        [HttpPost("AddAccount")]
        public async Task<IActionResult> AddAccount(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;

            MessageContainer responseAcc = await proxy.CreateAccount(requestMessage);
            MessageContainer response = new MessageContainer();

            response.Add("Account", responseAcc.Get<DTOAccount>());
            unitOfWork.Commit();

            return Ok(response);
        }
    }
}