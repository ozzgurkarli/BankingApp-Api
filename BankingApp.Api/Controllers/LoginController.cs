﻿using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IService _proxy;

        public LoginController(IService proxy)
        {
            _proxy = proxy;
        }

        [HttpGet("GetLoginCredentials")]
        public async Task<IActionResult> GetLoginCredentials([FromQuery] string identityNo)
        {
            MessageContainer requestMessage = new MessageContainer();
            MessageContainer responseMessage = new MessageContainer();
            requestMessage.Add(new DTOLogin { IdentityNo = identityNo});

            responseMessage = await _proxy.GetLoginCredentials(requestMessage);

            return Ok(responseMessage);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] DTOLogin dtoLogin)
        {
            MessageContainer requestMessage = new MessageContainer();
            MessageContainer response = new MessageContainer();

            requestMessage.Add(dtoLogin);
            response = await _proxy.UpdatePassword(requestMessage);

            return Ok(response);
        }
    }
}
