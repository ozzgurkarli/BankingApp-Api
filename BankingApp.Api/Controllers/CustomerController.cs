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

        [HttpPost("GetCustomerByIdentityNo")]
        public async Task<IActionResult> GetCustomerByIdentityNo(MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer();
            DTOCustomer dtoCustomer = message.ToObject<DTOCustomer>(message, "DTOCustomer");
            requestMessage.Add(dtoCustomer);

            return Ok(await _proxy.GetCustomerByIdentityNo(requestMessage));
        }


        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer();

            DTOCustomer customer = message.ToObject<DTOCustomer>(message, "DTOCustomer");
            requestMessage.Add(new DTOMailAddresses { MailAddress = customer.PrimaryMailAddress!, CustomerNo = "1" });
            requestMessage.Add(new DTOLogin { IdentityNo = customer.IdentityNo! });

            try
            {
                await _proxy.RegisterCheckDataAlreadyInUse(requestMessage);
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
