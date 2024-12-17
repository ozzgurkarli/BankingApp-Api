﻿using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IService proxy, IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpPost("GetCustomerByIdentityNo")]
        public async Task<IActionResult> GetCustomerByIdentityNo(MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer(unitOfWork);
            DTOCustomer dtoCustomer = message.ToObject<DTOCustomer>(message, "DTOCustomer");
            requestMessage.Add(dtoCustomer);

            MessageContainer responseMessage = await proxy.GetCustomerByIdentityNo(requestMessage);
            unitOfWork.Commit();

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(MessageContainer message)
        {
            MessageContainer requestMessage = new MessageContainer();

            DTOCustomer customer = message.ToObject<DTOCustomer>(message, "DTOCustomer");
            requestMessage.Add(new DTOMailAddresses { MailAddress = customer.PrimaryMailAddress!, CustomerNo = "1" });
            requestMessage.Add(new DTOLogin { IdentityNo = customer.IdentityNo! });

            try
            {
                await proxy.RegisterCheckDataAlreadyInUse(requestMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            message.Clear();
            message.Add(customer);

            return Ok(await proxy.CreateCustomer(message));
        }
    }
}