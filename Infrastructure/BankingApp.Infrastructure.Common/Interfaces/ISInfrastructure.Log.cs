using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Infrastructure.Common.Interfaces;

public partial interface ISInfrastructure
{
    public Task<MessageContainer> LogAPICall(MessageContainer request);
}