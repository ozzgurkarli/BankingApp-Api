using BankingApp.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using Npgsql;

namespace BankingApp.Infrastructure.Entity;

public class EOperation(IUnitOfWork unitOfWork)
{
    public DTOOperation Select(DTOOperation dto)
    {
        using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(""))
        {
            command.Parameters.AddWithValue("p_service", dto.ServiceName!);
            command.Parameters.AddWithValue("p_operation", dto.ServiceName!);
        }
    }
}