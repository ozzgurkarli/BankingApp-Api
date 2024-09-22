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
    public class TransactionHistoryController : ControllerBase
    {
        private readonly IService _proxy;
        public TransactionHistoryController(IService proxy)
        {
            _proxy = proxy;
        }
        [HttpGet("GetTransactionHistory")]
        public async Task<IActionResult> GetTransactionHistory([FromBody] MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer();
            MessageContainer responseMessage = new MessageContainer();
            requestMessage.Add(message.ToObject<DTOTransactionHistory>(message, "DTOTransactionHistory"));

            responseMessage = await _proxy.GetHistoryByFilter(requestMessage);

            return Ok(responseMessage);
        }
    }
}