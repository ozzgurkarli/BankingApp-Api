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
using BankingApp.Common.Interfaces;

namespace BankingApp.Entity
{
    public class ECustomer(IUnitOfWork unitOfWork)
    {
        public async Task<DTOCustomer> Add(DTOCustomer item)
        {
            long now = DateTime.UtcNow.Ticks;
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT i_customer(@refcursor, @p_recorddate, @p_recordscreen, @p_identityno, @p_name, @p_surname, @p_phoneno, @p_gender, @p_profession, @p_salary)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                command.Parameters.AddWithValue("p_identityno", item.IdentityNo!);
                command.Parameters.AddWithValue("p_name", item.Name!);
                command.Parameters.AddWithValue("p_surname", item.Surname!);
                command.Parameters.AddWithValue("p_phoneno", item.PhoneNo!);
                command.Parameters.AddWithValue("p_gender", item.Gender!);
                command.Parameters.AddWithValue("p_profession", item.Profession!);
                command.Parameters.AddWithValue("p_salary", item.Salary!);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
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

            return item;
        }

        public async Task<DTOCustomer> Get(DTOCustomer item)
        {
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand("SELECT s_customer(@refcursor, @p_id, @p_identityno)"))
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

            return item;
        }

        public async Task<DTOCustomer> GetByAccountNo(DTOCustomer item)
        {
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT s_customerinfosbyaccountno(@refcursor, @p_accountno)"))
            {
                command.Parameters.AddWithValue("p_accountno", item.AccountNo ?? (object)DBNull.Value);
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
                            CustomerNo = (string?)reader["Id"].ToString()
                        };
                    }
                }
            }

            return item;
        }
    }
}