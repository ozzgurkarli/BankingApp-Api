using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using BankingApp.Infrastructure.Entity;

namespace BankingApp.Infrastructure.Service;

public partial class SInfrastructure : ISInfrastructure
{
    public async Task<MessageContainer> SelectOperationWithName(MessageContainer requestMessage)
    {
        MessageContainer response = new MessageContainer();
        EOperation eOperation = new EOperation(requestMessage.UnitOfWork!);
        DTOOperation dtoOperation = requestMessage.Get<DTOOperation>()!;

        try
        {
            dtoOperation = await eOperation.SelectWithOperationName(dtoOperation);

            if (dtoOperation.Id == 0 || (bool)!dtoOperation.Active!)
            {
                throw new Exception("OPERASYON TANIMI BULUNAMADI");
            }

            response.Add(dtoOperation);
        }
        catch (Exception)
        {
            // xd
        }
        return response;
    }
}