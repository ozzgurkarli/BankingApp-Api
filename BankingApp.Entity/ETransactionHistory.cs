using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BankingApp.Entity
{
    public class ETransactionHistory
    {
        public async Task<List<DTOTransactionHistory>> Get(DTOTransactionHistory item)
        {
            List<DTOTransactionHistory> transactionList = new List<DTOTransactionHistory>();

            using(var connection = new NpgsqlConnection(ENV.DatabaseConnectionString)){
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                using(var command = new NpgsqlCommand("SELECT l_transactionhistory(@refcursor, @customer_id, @p_mindate, @p_maxdate)", connection, tran)){
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
                    command.Parameters.AddWithValue("customer_id", item.CustomerNo!);
                    command.Parameters.AddWithValue("p_mindate", item.MinDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_maxdate", item.MaxDate ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "fetch all in \"ref\"";
                    command.CommandType = System.Data.CommandType.Text;

                    using(var reader = await command.ExecuteReaderAsync()){
                        while (await reader.ReadAsync())
                        {
                            transactionList.Add(new DTOTransactionHistory{
                                Id =  reader.GetInt32(0),
                                AccountNo = reader.GetString(1),
                                CreditCardNo = reader.GetString(2),
                                Amount = reader.GetDecimal(3),
                                Currency = reader.GetString(4),
                                TransactionDate = reader.GetDateTime(5),
                                Description = reader.GetString(6),
                                CustomerNo = reader.GetInt64(7).ToString(),
                                TransactionType = reader.GetInt32(8)
                            });
                        }
                    }
                }

                await tran.DisposeAsync();
                await connection.CloseAsync();
            }
            
            return transactionList;
        }

        public async Task Add(DTOTransactionHistory item)
        {
            using(var connection = new NpgsqlConnection(ENV.DatabaseConnectionString)){
                await connection.OpenAsync();

                using(var command = new NpgsqlCommand("CALL i_transactionhistory(@p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_creditcardno, @p_amount, @p_currency, @p_transactiondate, @p_description, @p_transactiontype)", connection)){

                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!) );
                    command.Parameters.AddWithValue("p_accountno", item.AccountNo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_creditcardno", item.CreditCardNo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_amount", item.Amount!);
                    command.Parameters.AddWithValue("p_currency", item.Currency);
                    command.Parameters.AddWithValue("p_transactiondate", item.TransactionDate!);
                    command.Parameters.AddWithValue("p_description", item.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_transactiontype", item.TransactionType!);

                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }
    }
}