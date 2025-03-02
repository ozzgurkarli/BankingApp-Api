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
    public class TransferController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpPost("CheckRecipientCustomer")]
        public async Task<IActionResult> CheckRecipientCustomer(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            MessageContainer responseMessage = await proxy.CheckRecipientCustomer(requestMessage);
            unitOfWork.Commit();

            return Ok(responseMessage);
        }

        [HttpPost("StartTransfer")]
        public async Task<IActionResult> StartTransfer(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            MessageContainer responseMessage = await proxy.StartTransfer(requestMessage);
            unitOfWork.Commit();

            return Ok(responseMessage);
        }
    }
}