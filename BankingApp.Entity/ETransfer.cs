using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BankingApp.Entity
{
    public class ETransfer(IUnitOfWork unitOfWork)
    {
        public async Task<DTOTransfer> Add(DTOTransfer transfer)
        {
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT i_transfer(@refcursor, @p_recorddate, @p_recordscreen, @p_senderaccountno, @p_recipientaccountno, @p_currency, @p_status, @p_transactiondate, @p_orderdate, @p_amount)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", transfer.RecordScreen);
                command.Parameters.AddWithValue("p_senderaccountno", transfer.SenderAccountNo!);
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
            }

            return transfer;
        }

        public async Task<List<DTOTransfer>> GetOrdersToExecute(DTOTransfer item)
        {
            long now = DateTime.UtcNow.Ticks;
            List<DTOTransfer> transferList = new List<DTOTransfer>();
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand("SELECT l_transferstoexecute(@refcursor, @p_orderdate)"))
            {
                command.Parameters.AddWithValue("p_orderdate", DateTime.Today);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");
                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
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
                            SenderCustomerNo = ((Int64)reader["SenderCustomerId"]).ToString(),
                            SenderMailAddress = (string)reader["SenderMailAddress"],
                            SenderName = (string)reader["SenderCustomerName"],
                            RecipientAccountActive = await reader.IsDBNullAsync("RecipientAccountActive")
                                ? null
                                : (bool)reader["RecipientAccountActive"],
                            RecipientAccountNo = await reader.IsDBNullAsync("RecipientAccountNo")
                                ? null
                                : (string)reader["RecipientAccountNo"],
                            RecipientCustomerActive =
                                await reader.IsDBNullAsync("RecipientCustomerActive")
                                    ? null
                                    : (bool)reader["RecipientCustomerActive"],
                            RecipientCustomerNo =
                                await reader.IsDBNullAsync("RecipientCustomerId")
                                    ? null
                                    : ((Int64)reader["RecipientCustomerId"]).ToString(),
                            RecipientMailAddress =
                                await reader.IsDBNullAsync("RecipientMailAddress")
                                    ? null
                                    : (string)reader["RecipientMailAddress"],
                            RecipientName =
                                await reader.IsDBNullAsync("RecipientCustomerName")
                                    ? null
                                    : (string)reader["RecipientCustomerName"],
                            Amount = (decimal)reader["Amount"],
                            TransactionDate = (DateTime)reader["TransactionDate"],
                            OrderDate = (DateTime)reader["OrderDate"],
                            Currency = (string)reader["Currency"],
                            Status = (int)reader["Status"],
                        });
                    }
                }
            }

            return transferList;
        }

        public async Task<DTOTransfer> ExecuteTransfer(DTOTransfer transfer)
        {
            long now = DateTime.UtcNow.Ticks;
            DTOTransfer dtoTransfer = new DTOTransfer();
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT u_transfer(@refcursor, @p_id, @p_recorddate, @p_recordscreen, @p_senderaccountno, @p_recipientaccountno, @p_currency, @p_status, @p_transactiondate, @p_orderdate, @p_amount)"))
            {
                command.Parameters.AddWithValue("p_id", transfer.Id!);
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", transfer.RecordScreen);
                command.Parameters.AddWithValue("p_senderaccountno", transfer.SenderAccountNo!);
                command.Parameters.AddWithValue("p_recipientaccountno", transfer.RecipientAccountNo ?? "0000000000000000");
                command.Parameters.AddWithValue("p_status", transfer.Status!);
                command.Parameters.AddWithValue("p_transactiondate", transfer.TransactionDate!);
                command.Parameters.AddWithValue("p_currency", transfer.Currency!);
                command.Parameters.AddWithValue("p_orderdate", transfer.OrderDate!);
                command.Parameters.AddWithValue("p_amount", transfer.Amount!);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
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

            return dtoTransfer;
        }
    }
}