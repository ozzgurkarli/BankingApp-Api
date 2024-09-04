using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        
        private readonly IService _proxy;

        public AccountController(IService proxy)
        {
            _proxy = proxy;
        }

        [HttpPost("GetAccounts")]
        public async Task<IActionResult> GetAccounts(MessageContainer requestMessage){
            DTOAccount dtoAccount = requestMessage.ToObject<DTOAccount>(requestMessage, "DTOAccount");

            MessageContainer requestAccount = new MessageContainer();

            requestAccount.Add(dtoAccount);

            MessageContainer responseAcc = await _proxy.GetAccountsByFilter(requestAccount);
            MessageContainer response = new MessageContainer();

            response.Add("AccountList", responseAcc.Get<List<DTOAccount>>());

            return Ok(response);
        }
    }
}