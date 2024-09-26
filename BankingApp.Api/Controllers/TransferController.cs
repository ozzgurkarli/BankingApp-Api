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
    public class TransferController : ControllerBase
    {
        private readonly IService _proxy;
        public TransferController(IService proxy)
        {
            _proxy = proxy;
        }

        [HttpPost("StartTransfer")]
        public async Task<IActionResult> StartTransfer(MessageContainer message){
            DTOTransfer dtoTransfer = message.ToObject<DTOTransfer>(message, "DTOTransfer");

            MessageContainer requestMessage = new MessageContainer();

            requestMessage.Add(dtoTransfer);

            MessageContainer responseMessage = await _proxy.StartTransfer(requestMessage);

            return Ok(responseMessage);
        }
    }
}