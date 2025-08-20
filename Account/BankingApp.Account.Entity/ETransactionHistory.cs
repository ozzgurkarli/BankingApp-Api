using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Account.Common.DataTransferObjects;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using Npgsql;

namespace BankingApp.Account.Entity
{
    public class ETransactionHistory(IUnitOfWork _unitOfWork)
    {
        public async Task<List<DTOTransactionHistory>> Get(DTOTransactionHistory item)
        {
            List<DTOTransactionHistory> transactionList = new List<DTOTransactionHistory>();

            using (var command =
                   (NpgsqlCommand)_unitOfWork.CreateCommand(
                       "SELECT l_transactionhistory(@refcursor, @customer_id, @p_mindate, @p_maxdate, @p_count)"))
            {
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
                command.Parameters.AddWithValue("customer_id", item.CustomerNo!);
                command.Parameters.AddWithValue("p_mindate",
                    !item.MinDate.Equals(DateTime.MinValue) && item.MinDate != null
                        ? item.MinDate
                        : (object)DBNull.Value);
                command.Parameters.AddWithValue("p_maxdate",
                    !item.MaxDate.Equals(DateTime.MinValue) && item.MaxDate != null
                        ? item.MaxDate
                        : (object)DBNull.Value);
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
                            CreditCardNo = reader["CreditCardNo"] != DBNull.Value
                                ? reader["CreditCardNo"].ToString()
                                : null,
                            Amount = (decimal)reader["Amount"],
                            Currency = (string)reader["Currency"],
                            TransactionDate = (DateTime)reader["TransactionDate"],
                            Description = reader["Description"] != DBNull.Value
                                ? reader["Description"].ToString()
                                : null,
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString(),
                            TransactionType = (int)reader["TransactionType"]
                        });
                    }
                }
            }

            return transactionList;
        }

        public async Task<DTOTransactionHistory> Add(DTOTransactionHistory item)
        {
            using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                       "SELECT i_transactionhistory(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_creditcardno, @p_amount, @p_currency, @p_transactiondate, @p_description, @p_transactiontype)"))
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
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                    $"ref{item.AccountNo + item.CreditCardNo}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{item.AccountNo + item.CreditCardNo}\"";
                command.CommandType = System.Data.CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        item = new DTOTransactionHistory
                        {
                            Id = (int)reader["Id"],
                            AccountNo = reader["AccountNo"] != DBNull.Value ? (string?)reader["AccountNo"] : null,
                            CreditCardNo = reader["CreditCardNo"] != DBNull.Value
                                ? (string?)reader["CreditCardNo"]
                                : null,
                            Amount = (decimal)reader["Amount"],
                            Currency = (string)reader["Currency"],
                            TransactionDate = (DateTime)reader["TransactionDate"],
                            Description = reader["Description"] != DBNull.Value ? (string?)reader["Description"] : null,
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString(),
                            TransactionType = (int)reader["TransactionType"]
                        };
                    }
                }
            }

            return item;
        }

        public async Task<List<DTOTransactionHistory>> AddRange(List<DTOTransactionHistory> transactionList)
        {
            List<DTOTransactionHistory> dtoTHList = new List<DTOTransactionHistory>();
            int count = 0;
            foreach (var item in transactionList)
            {
                count++;
                using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                           "SELECT i_transactionhistory(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_creditcardno, @p_amount, @p_currency, @p_transactiondate, @p_description, @p_transactiontype)"))
                {
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                    command.Parameters.AddWithValue("p_accountno", item.AccountNo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_creditcardno",
                        item.CreditCardNo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_amount", item.Amount!);
                    command.Parameters.AddWithValue("p_currency", item.Currency);
                    command.Parameters.AddWithValue("p_transactiondate", item.TransactionDate!);
                    command.Parameters.AddWithValue("p_description", item.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_transactiontype", item.TransactionType!);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                        $"ref{count}");

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
                                AccountNo = reader["AccountNo"] != DBNull.Value
                                    ? (string?)reader["AccountNo"]
                                    : null,
                                CreditCardNo = reader["CreditCardNo"] != DBNull.Value
                                    ? (string?)reader["CreditCardNo"]
                                    : null,
                                Amount = (decimal)reader["Amount"],
                                Currency = (string)reader["Currency"],
                                TransactionDate = (DateTime)reader["TransactionDate"],
                                Description = reader["Description"] != DBNull.Value
                                    ? (string?)reader["Description"]
                                    : null,
                                CustomerNo = ((Int64)reader["CustomerId"]).ToString(),
                                TransactionType = (int)reader["TransactionType"]
                            });
                        }
                    }
                }
            }

            return dtoTHList;
        }
    }
}