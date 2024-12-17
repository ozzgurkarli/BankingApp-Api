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
    public class LoginController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("GetLoginCredentials")]
        public async Task<IActionResult> GetLoginCredentials([FromBody] MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer(unitOfWork);
            MessageContainer responseMessage = new MessageContainer();
            requestMessage.Add(message.ToObject<DTOLogin>(message, "DTOLogin"));

            responseMessage = await proxy.GetLoginCredentials(requestMessage);
            unitOfWork.Commit();

            return Ok(responseMessage);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer(unitOfWork);
            MessageContainer response = new MessageContainer();

            DTOLogin dtoLogin = message.ToObject<DTOLogin>(message, "DTOLogin");

            requestMessage.Add(dtoLogin);
            response = await proxy.UpdatePassword(requestMessage);
            unitOfWork.Commit();

            return Ok(response);
        }
    }
}