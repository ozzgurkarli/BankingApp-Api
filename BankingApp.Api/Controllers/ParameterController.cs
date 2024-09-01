﻿using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly IService _proxy;
        public ParameterController(IService proxy)
        {
            _proxy = proxy;
        }

        [HttpPost("GetParametersByGroupCode")]
        public async Task<IActionResult> GetParametersByGroupCode([FromBody] MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer();
            MessageContainer responseMessage = new MessageContainer();
            requestMessage.Add(message.ToObject<DTOParameter>(message, "Parameter"));

            responseMessage = await _proxy.GetParametersByGroupCode(requestMessage);

            return Ok(responseMessage);
        }
    }
}
