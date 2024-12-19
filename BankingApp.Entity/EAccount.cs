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
    public class EAccount(IUnitOfWork unitOfWork)
    {
        public async Task<DTOAccount> Add(DTOAccount item)
        {
            DTOAccount dtoAccount = new DTOAccount();
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT i_account(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_branch, @p_balance, @p_currency, @p_active, @p_primary)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                command.Parameters.AddWithValue("p_accountno", item.AccountNo!);
                command.Parameters.AddWithValue("p_branch", item.Branch!);
                command.Parameters.AddWithValue("p_balance", 0.0M);
                command.Parameters.AddWithValue("p_currency", item.Currency!);
                command.Parameters.AddWithValue("p_active", true);
                command.Parameters.AddWithValue("p_primary", item.Primary!);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dtoAccount = new DTOAccount
                        {
                            Id = reader.GetInt32(0),
                            CustomerNo = reader.GetInt64(1).ToString(),
                            AccountNo = reader.GetString(2),
                            Balance = reader.GetDecimal(3),
                            Currency = reader.GetString(4),
                            Active = reader.GetBoolean(5),
                            Primary = reader.GetBoolean(6),
                            Branch = reader.GetInt32(7)
                        };
                    }
                }
            }

            return dtoAccount;
        }

        public async Task<List<DTOAccount>> UpdateRange(List<DTOAccount> accList)
        {
            long now = DateTime.UtcNow.Ticks;
            List<DTOAccount> dtoAccountList = new List<DTOAccount>();
            foreach (var item in accList)
            {
                now = DateTime.UtcNow.Ticks;
                using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                           "SELECT u_account(@refcursor, @p_recorddate, @p_recordscreen, @p_id, @p_customerid, @p_accountno, @p_branch, @p_balance, @p_currency, @p_active, @p_primary)"))
                {
                    command.Parameters.AddWithValue("p_id", item.Id!);
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                    command.Parameters.AddWithValue("p_accountno", item.AccountNo!);
                    command.Parameters.AddWithValue("p_branch", item.Branch!);
                    command.Parameters.AddWithValue("p_balance", item.Balance!);
                    command.Parameters.AddWithValue("p_currency", item.Currency!);
                    command.Parameters.AddWithValue("p_active", true);
                    command.Parameters.AddWithValue("p_primary", item.Primary!);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                        $"ref{item.AccountNo}{now}");

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"fetch all in \"ref{item.AccountNo}{now}\"";
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dtoAccountList.Add(new DTOAccount
                            {
                                Id = reader.GetInt32(0),
                                CustomerNo = reader.GetInt64(1).ToString(),
                                AccountNo = reader.GetString(2),
                                Balance = reader.GetDecimal(3),
                                Currency = reader.GetString(4),
                                Active = reader.GetBoolean(5),
                                Primary = reader.GetBoolean(6),
                                Branch = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }

            return dtoAccountList;
        }

        public async Task<List<DTOAccount>> Get(DTOAccount acc)
        {
            long now = DateTime.UtcNow.Ticks;
            List<DTOAccount> accList = new List<DTOAccount>();
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT l_account(@refcursor, @p_id, @p_customerid, @p_accountno, @p_branch, @p_currency, @p_active, @p_primary)"))
            {
                command.Parameters.AddWithValue("p_id", acc.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_customerid",
                    acc.CustomerNo != null ? Int64.Parse(acc.CustomerNo!) : (object)DBNull.Value);
                command.Parameters.AddWithValue("p_accountno", acc.AccountNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_branch", acc.Branch ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_currency", acc.Currency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_active", acc.Active ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_primary", acc.Primary ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                    $"ref{acc.AccountNo}{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{acc.AccountNo}{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        accList.Add(new DTOAccount
                        {
                            Id = reader.GetInt32(0),
                            CustomerNo = reader.GetInt64(1).ToString(),
                            AccountNo = reader.GetString(2),
                            Balance = reader.GetDecimal(3),
                            Currency = reader.GetString(4),
                            Active = reader.GetBoolean(5),
                            Primary = reader.GetBoolean(6),
                            Branch = reader.GetInt32(7)
                        });
                    }
                }
            }

            return accList;
        }

        public async Task<DTOAccount> GetByCustomerIdentityNo(DTOAccount acc)
        {
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT l_accountbycustomeridentityno(@refcursor, @p_identityno, @p_currency, @p_active, @p_primary)"))
            {
                command.Parameters.AddWithValue("p_identityno", acc.CustomerIdentityNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_currency", acc.Currency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_active", acc.Active ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_primary", acc.Primary ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                    $"ref{acc.AccountNo}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{acc.AccountNo}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        acc = new DTOAccount
                        {
                            Id = reader.GetInt32(0),
                            CustomerNo = reader.GetInt64(1).ToString(),
                            AccountNo = reader.GetString(2),
                            Balance = reader.GetDecimal(3),
                            Currency = reader.GetString(4),
                            Active = reader.GetBoolean(5),
                            Primary = reader.GetBoolean(6),
                            Branch = reader.GetInt32(7)
                        };
                    }
                }
            }

            return acc;
        }

        public async Task<DTOAccount> GetFirstAvailableNoAndIncrease(DTOAccount acc)
        {
            DTOAccount dtoAccount = new DTOAccount();

            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand("SELECT u_accounttracker(@refcursor, @p_currencycode)"))
            {
                command.Parameters.AddWithValue("p_currencycode", acc.Currency!);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dtoAccount.AccountNo = reader.GetString(0);
                    }
                }
            }


            return dtoAccount;
        }
    }
}