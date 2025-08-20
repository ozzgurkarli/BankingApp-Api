using System.Data;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using Npgsql;

namespace BankingApp.Infrastructure.Entity;

public class EOperation(IUnitOfWork unitOfWork)
{
    public async Task<DTOOperation> SelectWithOperationName(DTOOperation dto)
    {
        DTOOperation dtoOperation = new DTOOperation();
        
        using (var command = (NpgsqlCommand)unitOfWork.CreateCommand("SELECT s_operationwithname(@refcursor, @p_service, @p_operation)"))
        {
            command.Parameters.AddWithValue("p_service", dto.ServiceName!);
            command.Parameters.AddWithValue("p_operation", dto.ServiceName!);
            command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
            
            command.CommandText = "fetch all in \"ref\"";
            command.CommandType = CommandType.Text;

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    dtoOperation = new DTOOperation()
                    {
                        Id = (int)reader["Id"],
                        ServiceName = (string?)reader["Service"],
                        OperationName = (string?)reader["Operation"],
                        Active = (bool?)reader["Active"],
                    };
                }
            }
        }

        return dtoOperation;
    }
}