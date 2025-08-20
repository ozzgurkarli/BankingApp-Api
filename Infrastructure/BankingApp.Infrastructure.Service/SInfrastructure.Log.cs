using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using BankingApp.Infrastructure.Entity;

namespace BankingApp.Infrastructure.Service;

public partial class SInfrastructure: ISInfrastructure
{
    public async Task<MessageContainer> LogAPICall(MessageContainer request)
    {
        MessageContainer response = new MessageContainer();
        ELog eLog = new ELog(request.UnitOfWork!);
        DTOLog log = new DTOLog
        {
            Operation = string.Concat(request.CallerInformation.ServiceName, '.', request.CallerInformation.OperationName),
            APICall =true,
            
        };

        try
        {
            response.Add(await eLog.Insert(log));
        }
        catch (Exception)
        {
            // xd
        }

        return response;
    }
}