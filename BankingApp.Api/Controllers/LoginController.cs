using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BankingApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IService _proxy;

        public LoginController(IService proxy)
        {
            _proxy = proxy;
        }

        [AllowAnonymous]
        [HttpPost("GetLoginCredentials")]
        public async Task<IActionResult> GetLoginCredentials([FromBody] MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer();
            MessageContainer responseMessage = new MessageContainer();
            requestMessage.Add(message.ToObject<DTOLogin>(message, "DTOLogin"));

            responseMessage = await _proxy.GetLoginCredentials(requestMessage);

            return Ok(responseMessage);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer();
            MessageContainer response = new MessageContainer();

            DTOLogin dtoLogin = message.ToObject<DTOLogin>(message, "DTOLogin");

            requestMessage.Add(dtoLogin);
            response = await _proxy.UpdatePassword(requestMessage);

            return Ok(response);
        }
    }
}
