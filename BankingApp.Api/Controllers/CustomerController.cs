using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IServiceProvider ServiceProvider, IUnitOfWork unitOfWork) : ControllerBase
    {
        private IService reer = ServiceProvider.GetRequiredService<IService>();
        [HttpPost("GetCustomerByIdentityNo")]
        public async Task<IActionResult> GetCustomerByIdentityNo(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            MessageContainer responseMessage;
            using (var proxy = ServiceProvider.GetRequiredService<IService>())
            {
                responseMessage = await proxy.GetCustomerByIdentityNo(requestMessage);
            }
            unitOfWork.Commit();

            return Ok(responseMessage);
        }


        [AllowAnonymous]
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(MessageContainer requestMessage)
        {
            requestMessage.UnitOfWork = unitOfWork;
            DTOCustomer customer = requestMessage.Get<DTOCustomer>();
            requestMessage.Add(new DTOMailAddresses { MailAddress = customer.PrimaryMailAddress!, CustomerNo = "1" });
            requestMessage.Add(new DTOLogin { IdentityNo = customer.IdentityNo! });

            MessageContainer responseMessage;
            try
            {
                using (var proxy = ServiceProvider.GetRequiredService<IService>())
                {
                    await proxy.RegisterCheckDataAlreadyInUse(requestMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            using (var proxy = ServiceProvider.GetRequiredService<IService>())
            {
                responseMessage = await proxy.CreateCustomer(requestMessage);
            }
            unitOfWork.Commit();

            return Ok(responseMessage);
        }
    }
}