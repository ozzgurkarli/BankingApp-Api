using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers;

[Authorize]
[Route("api")]
[ApiController]
public class RequestController(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : ControllerBase
{
    [HttpPost("InvokeOperation")]
    public async Task<MessageContainer> InvokeOperation(MessageContainer requestMessage)
    {
        requestMessage.UnitOfWork = unitOfWork;

        using (var proxy = serviceProvider.GetRequiredService<ISInfrastructure>())
        {
            await proxy.LogAPICall(requestMessage);
        }

        var interfaceType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .FirstOrDefault(x => x.IsInterface && x.Name == requestMessage.CallerInformation.ServiceName);

        if (interfaceType == null)
        {
            throw new Exception($"I{requestMessage.CallerInformation.ServiceName} arayüzü bulunamadı.");
        }
        var service = serviceProvider.GetRequiredService(interfaceType);
        var method = interfaceType.GetMethod(requestMessage.CallerInformation.OperationName!);
        
        if (method == null)
        {
            throw new Exception($"I{requestMessage.CallerInformation.ServiceName}.{requestMessage.CallerInformation.OperationName} 'operasyonu bulunamadı.");
        }

        MessageContainer responseMessage = (MessageContainer)method.Invoke(service, new object?[]{requestMessage})!;

        return responseMessage;
    }
}