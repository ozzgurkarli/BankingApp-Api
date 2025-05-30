using System.Data;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using Npgsql;

namespace BankingApp.Infrastructure.Entity;

public class ELog(IUnitOfWork unitOfWork)
{
    public async DTOLog Insert(DTOLog dto)
    {
        long now = DateTime.UtcNow.Ticks;

        using (var command = (NpgsqlCommand)unitOfWork.CreateCommand("SELECT i_log(@refcursor, @p_recorddate, @p_recordscreen, @p_operation, @p_apicall, @p_request, @p_response, @p_errormessage, @p_callerip, @p_caller, @p_transactionid)"))
        {
            command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
            command.Parameters.AddWithValue("p_recordscreen", dto.RecordScreen);
            command.Parameters.AddWithValue("p_operation", Int64.Parse(dto.Operation!));
            command.Parameters.AddWithValue("p_apicall", dto.APICall);
            command.Parameters.AddWithValue("p_request", dto.Request!);
            command.Parameters.AddWithValue("p_response", dto.Response!);
            command.Parameters.AddWithValue("p_errormessage", dto.ErrorMessage!);
            command.Parameters.AddWithValue("p_callerip", dto.CallerIP!);
            command.Parameters.AddWithValue("p_caller", dto.Caller!);
            command.Parameters.AddWithValue("p_transactionid", unitOfWork.TransactionId);
            command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

            await command.ExecuteNonQueryAsync();
            
            command.CommandText = $"fetch all in \"ref{now}\"";
            command.CommandType = CommandType.Text;
        }
    }
}