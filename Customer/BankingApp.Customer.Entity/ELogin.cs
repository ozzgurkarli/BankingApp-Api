using BankingApp.Common.DataTransferObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.Interfaces;
using BankingApp.Customer.Common.DataTransferObjects;

namespace BankingApp.Customer.Entity
{
    public class ELogin(IUnitOfWork unitOfWork)
    {
        public async Task<DTOLogin> Add(DTOLogin item)
        {
            long now = DateTime.UtcNow.Ticks;
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT i_login(@refcursor, @p_recorddate, @p_recordscreen, @p_identityno, @p_password, @p_temporary, @p_notificationtoken)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                command.Parameters.AddWithValue("p_identityno", item.IdentityNo!);
                command.Parameters.AddWithValue("p_password", item.Password!);
                command.Parameters.AddWithValue("p_temporary", item.Temporary!);
                command.Parameters.AddWithValue("p_notificationtoken", item.NotificationToken ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        item = new DTOLogin
                        {
                            Id = (int)reader["Id"],
                            Password = (string)reader["Password"],
                            IdentityNo = (string)reader["IdentityNo"],
                            Temporary = (bool)reader["Temporary"]
                        };
                    }
                }
            }

            return item;
        }

        public async Task<DTOLogin> Update(DTOLogin item)
        {
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT u_login(@refcursor, @p_recorddate, @p_recordscreen, @p_id, @p_identityno, @p_password, @p_temporary, @p_notificationtoken)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                command.Parameters.AddWithValue("p_id", item.Id!);
                command.Parameters.AddWithValue("p_identityno", item.IdentityNo!);
                command.Parameters.AddWithValue("p_password", item.Password!);
                command.Parameters.AddWithValue("p_temporary", item.Temporary!);
                command.Parameters.AddWithValue("p_notificationtoken", item.NotificationToken ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        item = new DTOLogin
                        {
                            Id = (int)reader["Id"],
                            Password = (string)reader["Password"],
                            IdentityNo = (string)reader["IdentityNo"],
                            Temporary = (bool)reader["Temporary"]
                        };
                    }
                }
            }


            return item;
        }

        public async Task<DTOLogin?> Select(DTOLogin item)
        {
            long now = DateTime.UtcNow.Ticks;
            DTOLogin? dtoLogin = null;
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand("SELECT s_login(@refcursor, @p_id, @p_identityno)"))
            {
                command.Parameters.AddWithValue("p_id", item.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_identityno", item.IdentityNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dtoLogin = new DTOLogin
                        {
                            Id = (int)reader["Id"],
                            Password = (string?)reader["Password"].ToString(),
                            IdentityNo = (string)reader["IdentityNo"],
                            Temporary = (bool)reader["Temporary"],
                            NotificationToken = await reader.IsDBNullAsync("NotificationToken")
                                ? null
                                : (string?)reader["NotificationToken"],
                            CustomerNo = ((Int64)reader["CustomerNo"]).ToString()
                        };
                    }
                }
            }

            return dtoLogin;
        }

        public async Task<DTOLogin> GetNotificationToken(DTOLogin item)
        {
            long now = DateTime.UtcNow.Ticks;
            DTOLogin dtoLogin = null;
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT s_notificationtoken(@refcursor, @p_id, @p_identityno, @p_customerid)"))
            {
                command.Parameters.AddWithValue("p_id", item.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_identityno", item.IdentityNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_customerid",
                    item.CustomerNo != null ? Int64.Parse(item.CustomerNo!) : (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dtoLogin = new DTOLogin
                        {
                            Id = (int)reader["Id"],
                            IdentityNo = (string)reader["IdentityNo"],
                            NotificationToken = await reader.IsDBNullAsync("NotificationToken")
                                ? null
                                : (string?)reader["NotificationToken"]
                        };
                    }
                }
            }

            return dtoLogin;
        }
    }
}