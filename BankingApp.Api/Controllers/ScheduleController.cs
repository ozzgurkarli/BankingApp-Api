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
    public class ScheduleController : ControllerBase
    {
        private readonly IService _proxy;
        public ScheduleController(IService proxy)
        {
            _proxy = proxy;
        }

        [HttpPost("TriggerSchedules")]
        public async Task<IActionResult> TriggerSchedules(){

            _proxy.SetCurrencyValues(new MessageContainer());
            _proxy.ExecuteTransferSchedule(new MessageContainer());
            
            return Ok(new MessageContainer());
        }
    }
}