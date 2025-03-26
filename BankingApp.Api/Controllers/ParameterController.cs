using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("GetParametersByGroupCode")]
        public async Task<IActionResult> GetParametersByGroupCode([FromBody] MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            MessageContainer responseMessage = new MessageContainer();

            responseMessage = await proxy.GetParametersByGroupCode(requestMessage);
            unitOfWork.Commit();

            return Ok(responseMessage);
        }

        [AllowAnonymous]
        [HttpPost("GetMultipleGroupCode")]
        public async Task<IActionResult> GetMultipleGroupCode([FromBody] MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            MessageContainer responseMessage = new MessageContainer();

            responseMessage = await proxy.GetMultipleGroupCode(requestMessage);
            unitOfWork.Commit();

            return Ok(responseMessage);
        }
    }
}