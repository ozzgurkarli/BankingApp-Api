using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity
{
    public class ECustomer
    {
        public async Task<DTOCustomer> Add(DTOCustomer item)
        {
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();
                try
                {
                    using (var command = new NpgsqlCommand("SELECT s_customer(@refcursor, @p_id, @p_identityno)", connection)){
                        command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                        command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                        command.Parameters.AddWithValue("p_identityno", item.IdentityNo!);
                        command.Parameters.AddWithValue("p_name", item.Name!);
                        command.Parameters.AddWithValue("p_surname", item.Surname!);
                        command.Parameters.AddWithValue("p_phoneno", item.PhoneNo!);
                        command.Parameters.AddWithValue("p_gender", item.Gender!);
                        command.Parameters.AddWithValue("p_profession", item.Profession!);
                        command.Parameters.AddWithValue("p_salary", item.Salary!);
                        command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                        await command.ExecuteNonQueryAsync();

                        command.CommandText = "fetch all in \"ref\"";
                        command.CommandType = CommandType.Text;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                item = new DTOCustomer
                                {
                                    Id = reader.GetInt64(0),
                                    IdentityNo = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Surname = reader.GetString(3),
                                    Gender = reader.GetInt32(4),
                                    Active = reader.GetBoolean(5),
                                    PhoneNo = reader.GetString(6),
                                    Salary = reader.GetDecimal(7),
                                    CreditScore = reader.GetInt32(8),
                                    Profession = reader.GetInt32(9),
                                    CustomerNo = reader.GetInt64(0).ToString(),
                                    PrimaryMailAddress = item.PrimaryMailAddress
                                };
                            }
                        }
                    }

                    await tran.CommitAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                    throw;
                }
                finally
                {
                    await tran.DisposeAsync();
                    await connection.CloseAsync();
                }
            }

            return item;
        }

        public async Task<DTOCustomer> Get(DTOCustomer item)
        {
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                using (var command = new NpgsqlCommand("SELECT s_customer(@refcursor, @p_id, @p_identityno)", connection))
                {
                    command.Parameters.AddWithValue("p_id", item.Id ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_identityno", item.IdentityNo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "fetch all in \"ref\"";
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            item = new DTOCustomer
                            {
                                Id = (Int64)reader["Id"],
                                IdentityNo = (string?)reader["IdentityNo"],
                                Name = (string?)reader["Name"],
                                Surname = (string?)reader["Surname"],
                                Gender = (int?)reader["Gender"],
                                Active = (bool?)reader["Active"],
                                PhoneNo = (string?)reader["PhoneNo"],
                                Salary = (decimal?)reader["Salary"],
                                CreditScore = (int?)reader["CreditScore"],
                                Profession = (int?)reader["Profession"],
                                CustomerNo = (string?)reader["Id"].ToString(),
                                PrimaryMailAddress = item.PrimaryMailAddress
                            };
                        }
                    }
                }
                await tran.DisposeAsync();
                await connection.CloseAsync();
            }

            return item;
        }
    }
}
