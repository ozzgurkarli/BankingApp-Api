using BankingApp.Common.DataTransferObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Customer.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;

namespace BankingApp.Customer.Entity
{
    public class EMailAddresses(IUnitOfWork unitOfWork)
    {
        public async Task Add(DTOMailAddresses item)
        {
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "CALL i_mailaddress(@p_recorddate, @p_recordscreen, @p_customerid, @p_mailaddress, @p_primary)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                command.Parameters.AddWithValue("p_mailaddress", item.MailAddress!);
                command.Parameters.AddWithValue("p_primary", item.Primary!);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<DTOMailAddresses?> Get(DTOMailAddresses item)
        {
            DTOMailAddresses? mailAddress = null;
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT l_emailaddress(@refcursor, @p_customerid, @p_mailaddress, @p_primary)"))
            {
                command.Parameters.AddWithValue("p_customerid",
                    item.CustomerNo != null ? Int64.Parse(item.CustomerNo) : (object)DBNull.Value);
                command.Parameters.AddWithValue("p_mailaddress", item.MailAddress ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_primary", item.Primary ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        mailAddress = new DTOMailAddresses
                        {
                            Id = (int)reader["Id"],
                            MailAddress = (string?)reader["MailAddress"],
                            Primary = (bool?)reader["Primary"],
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString()
                        };
                    }
                }
            }

            return mailAddress;
        }
    }
}