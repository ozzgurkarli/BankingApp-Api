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
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpPost("TriggerSchedules")]
        public async Task<IActionResult> TriggerSchedules()
        {
            await proxy.AccountClosingSchedule(new MessageContainer(unitOfWork));
            await proxy.ExecuteInstallmentSchedule(new MessageContainer(unitOfWork));
            await proxy.CardRevenuePaymentSchedule(new MessageContainer(unitOfWork));
            await proxy.SetCurrencyValuesSchedule(new MessageContainer(unitOfWork));
            await proxy.ExecuteTransferSchedule(new MessageContainer(unitOfWork));
            unitOfWork.Commit();

            return Ok(new MessageContainer());
        }
    }
}