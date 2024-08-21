using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IService _proxy;

        public CustomerController(IService proxy)
        {
            _proxy = proxy;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Post(DTOCustomer customer)
        {
            MessageContainer message = new MessageContainer();
            message.Add(new DTOMailAddresses { MailAddress = customer.PrimaryMailAddress!, CustomerNo = "1" });
            message.Add(new DTOLogin { IdentityNo = customer.IdentityNo! });

            try
            {
                await _proxy.RegisterCheckDataAlreadyInUse(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            message.Clear();
            message.Add(customer);

            return Ok(await _proxy.CreateCustomer(message));
        }
    }
}
