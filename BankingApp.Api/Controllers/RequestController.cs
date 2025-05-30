using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers;

[Authorize]
[Route("api")]
[ApiController]
public class RequestController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpPost("InvokeOperation")]
    public async Task<MessageContainer> InvokeOperation(MessageContainer requestMessage)
    {
        requestMessage.UnitOfWork = unitOfWork;
        requestMessage.CallerInformation.
    }
}