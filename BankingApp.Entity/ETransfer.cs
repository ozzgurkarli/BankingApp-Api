using System;
using System.Collections.Generic;
using System.Data;
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
    public class ETransfer
    {
        private readonly BankingDbContext database = new BankingDbContext();

        public async Task<DTOTransfer> Add(DTOTransfer transfer)
        {
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                using (var command = new NpgsqlCommand("SELECT i_transfer(@refcursor, @p_recorddate, @p_recordscreen, @p_senderaccountno, @p_recipientaccountno, @p_currency, @p_status, @p_transactiondate, @p_orderdate, @p_amount)", connection, tran))
                {
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", transfer.RecordScreen);
                    command.Parameters.AddWithValue("p_senderaccountno", Int64.Parse(transfer.SenderAccountNo!));
                    command.Parameters.AddWithValue("p_recipientaccountno", transfer.RecipientAccountNo!);
                    command.Parameters.AddWithValue("p_status", transfer.Status!);
                    command.Parameters.AddWithValue("p_transactiondate", transfer.TransactionDate!);
                    command.Parameters.AddWithValue("p_currency", transfer.Currency!);
                    command.Parameters.AddWithValue("p_orderdate", transfer.OrderDate!);
                    command.Parameters.AddWithValue("p_amount", transfer.Amount!);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "fetch all in \"ref\"";
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            transfer = new DTOTransfer
                            {
                                Id = (int)reader["Id"],
                                SenderAccountNo = (string)reader["SenderAccountNo"],
                                RecipientAccountNo = (string)reader["SenderAccountNo"],
                                Amount = (decimal)reader["Amount"],
                                TransactionDate = (DateTime)reader["TransactionDate"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                Currency = (string)reader["Currency"],
                                Status = (int)reader["Status"],
                            };
                        }
                    }
                    await tran.CommitAsync();
                    await tran.DisposeAsync();
                    await connection.CloseAsync();
                }
            }

            return transfer;
        }

        public async Task<Transfer> Update(Transfer item)
        {
            using (var context = new BankingDbContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.Entry(item.SenderAccount).State = EntityState.Unchanged;
                context.Entry(item.RecipientAccount).State = EntityState.Unchanged;
                context.Transfer.UpdateRange(item);

                await context.SaveChangesAsync();
            }

            return item;
        }

        public async Task<List<DTOTransfer>> GetOrdersToExecute(DTOTransfer item)
        {
            List<DTOTransfer> transferList = new List<DTOTransfer>();
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                using (var command = new NpgsqlCommand("SELECT l_transferstoexecute(@refcursor, @p_orderdate)", connection, tran))
                {
                    command.Parameters.AddWithValue("p_orderdate", DateTime.Today);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "fetch all in \"ref\"";
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            transferList.Add(new DTOTransfer
                            {
                                Id = (int)reader["Id"],
                                SenderAccountNo = (string)reader["SenderAccountNo"],
                                SenderAccountBalance = (decimal)reader["SenderAccountBalance"],
                                SenderAccountActive = (bool)reader["SenderAccountActive"],
                                SenderCustomerActive = (bool)reader["SenderCustomerActive"],
                                SenderCustomerNo = (string)reader["SenderCustomerId"],
                                SenderMailAddress = (string)reader["SenderMailAddress"],
                                SenderName = (string)reader["SenderCustomerName"],
                                RecipientAccountActive = (bool)reader["RecipientAccountActive"],
                                RecipientAccountNo = (string)reader["RecipientAccountNo"],
                                RecipientCustomerActive = (bool)reader["RecipientCustomerActive"],
                                RecipientCustomerNo = (string)reader["RecipientCustomerId"],
                                RecipientMailAddress = (string)reader["RecipientMailAddress"],
                                RecipientName = (string)reader["RecipientCustomerName"],
                                Amount = (decimal)reader["Amount"],
                                TransactionDate = (DateTime)reader["TransactionDate"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                Currency = (string)reader["Currency"],
                                Status = (int)reader["Status"],
                            });
                        }
                    }

                    await tran.DisposeAsync();
                    await connection.CloseAsync();
                }
            }
            return transferList;
        }

        public async Task<DTOTransfer> ExecuteTransfer(DTOTransfer transfer, List<DTOAccount> accountList, List<DTOTransactionHistory> transactionList)
        {
            DTOTransfer dtoTransfer = new DTOTransfer();
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                try
                {
                    using (var command = new NpgsqlCommand("SELECT u_transfer(@refcursor, @p_id, @p_recorddate, @p_recordscreen, @p_senderaccountno, @p_recipientaccountno, @p_currency, @p_status, @p_transactiondate, @p_orderdate, @p_amount)", connection, tran))
                    {
                        command.Parameters.AddWithValue("p_id", transfer.Id!);
                        command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                        command.Parameters.AddWithValue("p_recordscreen", transfer.RecordScreen);
                        command.Parameters.AddWithValue("p_senderaccountno", Int64.Parse(transfer.SenderAccountNo!));
                        command.Parameters.AddWithValue("p_recipientaccountno", transfer.RecipientAccountNo!);
                        command.Parameters.AddWithValue("p_status", transfer.Status!);
                        command.Parameters.AddWithValue("p_transactiondate", transfer.TransactionDate!);
                        command.Parameters.AddWithValue("p_currency", transfer.Currency!);
                        command.Parameters.AddWithValue("p_orderdate", transfer.OrderDate!);
                        command.Parameters.AddWithValue("p_amount", transfer.Amount!);
                        command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                        await command.ExecuteNonQueryAsync();

                        command.CommandText = "fetch all in \"ref\"";
                        command.CommandType = CommandType.Text;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                dtoTransfer = new DTOTransfer
                                {
                                    Id = (int)reader["Id"],
                                    SenderAccountNo = (string)reader["SenderAccountNo"],
                                    RecipientAccountNo = (string)reader["SenderAccountNo"],
                                    Amount = (decimal)reader["Amount"],
                                    TransactionDate = (DateTime)reader["TransactionDate"],
                                    OrderDate = (DateTime)reader["OrderDate"],
                                    Currency = (string)reader["Currency"],
                                    Status = (int)reader["Status"],
                                };
                            }
                        }
                    }

                    foreach (var item in accountList)
                    {
                        using (var command = new NpgsqlCommand("SELECT u_account(@refcursor, @p_recorddate, @p_recordscreen, @p_id, @p_customerid, @p_accountno, @p_branch, @p_balance, @p_currency, @p_active, @p_primary)", connection, tran))
                        {
                            command.Parameters.AddWithValue("p_id", item.Id!);
                            command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                            command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                            command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                            command.Parameters.AddWithValue("p_accountno", item.AccountNo!);
                            command.Parameters.AddWithValue("p_branch", item.Branch!);
                            command.Parameters.AddWithValue("p_balance", 0.0M);
                            command.Parameters.AddWithValue("p_currency", item.Currency!);
                            command.Parameters.AddWithValue("p_active", true);
                            command.Parameters.AddWithValue("p_primary", item.Primary!);
                            command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{item.AccountNo}");

                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    foreach (var item in transactionList)
                    {
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

            return dtoTransfer;
        }
    }
}