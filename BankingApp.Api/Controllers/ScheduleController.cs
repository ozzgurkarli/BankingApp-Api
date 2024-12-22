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
            await proxy.ScheduleManager(new MessageContainer(unitOfWork));
            unitOfWork.Commit();

            return Ok(new MessageContainer());
        }
    }
}