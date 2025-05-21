using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Infrastructure.Common.Interfaces;

public partial interface ISInfrastructure
{
    public Task<MessageContainer> SendMail(MessageContainer requestMessage);

    public Task<MessageContainer> SendNotification(MessageContainer requestMessage);
}