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
    public class TransactionHistoryController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpPost("GetTransactionHistory")]
        public async Task<IActionResult> GetTransactionHistory([FromBody] MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer(unitOfWork);
            MessageContainer responseMessage = new MessageContainer();
            requestMessage.Add(message.ToObject<DTOTransactionHistory>(message, "DTOTransactionHistory"));

            responseMessage = await proxy.GetHistoryByFilter(requestMessage);

            return Ok(responseMessage);
        }
    }
}