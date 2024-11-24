using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BankingApp.Entity
{
    public class ETransactionHistory
    {
        public async Task<List<DTOTransactionHistory>> Get(DTOTransactionHistory item)
        {
            List<DTOTransactionHistory> transactionList = new List<DTOTransactionHistory>();

            using (var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                using (var command = new NpgsqlCommand("SELECT l_transactionhistory(@refcursor, @customer_id, @p_mindate, @p_maxdate, @p_count)", connection, tran))
                {
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
                    command.Parameters.AddWithValue("customer_id", item.CustomerNo!);
                    command.Parameters.AddWithValue("p_mindate", !item.MinDate.Equals(DateTime.MinValue) && item.MinDate != null ? item.MinDate : (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_maxdate", !item.MaxDate.Equals(DateTime.MinValue) && item.MaxDate != null ? item.MaxDate : (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_count", item.Count ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "fetch all in \"ref\"";
                    command.CommandType = System.Data.CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            transactionList.Add(new DTOTransactionHistory
                            {
                                Id = (int)reader["Id"],
                                AccountNo = reader["AccountNo"] != DBNull.Value ? reader["AccountNo"].ToString() : null,
                                CreditCardNo = reader["CreditCardNo"] != DBNull.Value ? reader["CreditCardNo"].ToString() : null,
                                Amount = (decimal)reader["Amount"],
                                Currency = (string)reader["Currency"],
                                TransactionDate = (DateTime)reader["TransactionDate"],
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                CustomerNo = ((Int64)reader["CustomerId"]).ToString(),
                                TransactionType = (int)reader["TransactionType"]
                            });
                        }
                    }
                }

                await tran.DisposeAsync();
                await connection.CloseAsync();
            }

            return transactionList;
        }

        public async Task<DTOTransactionHistory> Add(DTOTransactionHistory item)
        {
            using (var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();
                using (var command = new NpgsqlCommand("SELECT i_transactionhistory(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_creditcardno, @p_amount, @p_currency, @p_transactiondate, @p_description, @p_transactiontype)", connection, tran))
                {

                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                    command.Parameters.AddWithValue("p_accountno", item.AccountNo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_creditcardno", item.CreditCardNo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_amount", item.Amount!);
                    command.Parameters.AddWithValue("p_currency", item.Currency);
                    command.Parameters.AddWithValue("p_transactiondate", item.TransactionDate!);
                    command.Parameters.AddWithValue("p_description", item.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_transactiontype", item.TransactionType!);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{item.AccountNo}");

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "fetch all in \"ref\"";
                    command.CommandType = System.Data.CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            item = new DTOTransactionHistory
                            {
                                Id = (int)reader["Id"],
                                AccountNo = reader["AccountNo"] != DBNull.Value ? (string?)reader["AccountNo"] : null,
                                CreditCardNo = reader["CreditCardNo"] != DBNull.Value ? (string?)reader["CreditCardNo"] : null,
                                Amount = (decimal)reader["Amount"],
                                Currency = (string)reader["Currency"],
                                TransactionDate = (DateTime)reader["TransactionDate"],
                                Description = reader["Description"] != DBNull.Value ? (string?)reader["Description"] : null,
                                CustomerNo = ((Int64)reader["CustomerId"]).ToString(),
                                TransactionType = (int)reader["TransactionType"]
                            };
                        }
                    }
                    await tran.CommitAsync();
                }

                await connection.CloseAsync();
            }

            return item;
        }

        public async Task<List<DTOTransactionHistory>> AddRange(List<DTOTransactionHistory> transactionList)
        {
            List<DTOTransactionHistory> dtoTHList = new List<DTOTransactionHistory>();
            using (var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                try
                {
                    int count = 0;
                    foreach (var item in transactionList)
                    {
                        count++;
                        using (var command = new NpgsqlCommand("SELECT i_transactionhistory(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_creditcardno, @p_amount, @p_currency, @p_transactiondate, @p_description, @p_transactiontype)", connection, tran))
                        {

                            command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                            command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                            command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                            command.Parameters.AddWithValue("p_accountno", item.AccountNo ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("p_creditcardno", item.CreditCardNo ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("p_amount", item.Amount!);
                            command.Parameters.AddWithValue("p_currency", item.Currency);
                            command.Parameters.AddWithValue("p_transactiondate", item.TransactionDate!);
                            command.Parameters.AddWithValue("p_description", item.Description ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("p_transactiontype", item.TransactionType!);
                            command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{count}");

                            await command.ExecuteNonQueryAsync();

                            command.CommandText = $"fetch all in \"ref{count}\"";
                            command.CommandType = System.Data.CommandType.Text;

                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    dtoTHList.Add(new DTOTransactionHistory
                                    {
                                        Id = (int)reader["Id"],
                                        AccountNo = reader["AccountNo"] != DBNull.Value ? (string?)reader["AccountNo"] : null,
                                        CreditCardNo = reader["CreditCardNo"] != DBNull.Value ? (string?)reader["CreditCardNo"] : null,
                                        Amount = (decimal)reader["Amount"],
                                        Currency = (string)reader["Currency"],
                                        TransactionDate = (DateTime)reader["TransactionDate"],
                                        Description = reader["Description"] != DBNull.Value ? (string?)reader["Description"] : null,
                                        CustomerNo = ((Int64)reader["CustomerId"]).ToString(),
                                        TransactionType = (int)reader["TransactionType"]
                                    });
                                }
                            }
                        }
                    }
                    await tran.CommitAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
                await tran.DisposeAsync();
                await connection.CloseAsync();
            }

            return dtoTHList;
        }
    }
}