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
        public async Task<IActionResult> GetLoginCredentials(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            MessageContainer responseMessage = new MessageContainer();

            responseMessage = await proxy.GetLoginCredentials(requestMessage);
            unitOfWork.Commit();

            return Ok(responseMessage);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            MessageContainer response = new MessageContainer();
            response = await proxy.UpdatePassword(requestMessage);
            unitOfWork.Commit();

            return Ok(response);
        }
    }
}