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
        private readonly IService _myService;

        public CustomerController(IService myService)
        {
            _myService = myService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Post(DTOCustomer customer)
        {
            MessageContainer message = new MessageContainer();
            message.Add(customer);

            message = _myService.CreateCustomer(message);

            return Ok(message);
        }
    }
}
