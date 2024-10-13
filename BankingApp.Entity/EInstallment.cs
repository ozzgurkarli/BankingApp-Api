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
    public class EInstallment
    {
        public async Task<List<DTOInstallment>> AddRange(List<DTOInstallment> installmentList)
        {
            List<DTOInstallment> dtoInstallmentList = new List<DTOInstallment>();
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                try
                {
                    foreach (var item in installmentList)
                    {
                        using (var command = new NpgsqlCommand("SELECT i_installment(@refcursor, @p_recorddate, @p_recordscreen, @p_cardno, @p_amount, @p_paymentdate, @p_installmentnumber)", connection, tran))
                        {
                            command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                            command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                            command.Parameters.AddWithValue("p_cardno", item.CreditCardNo!);
                            command.Parameters.AddWithValue("p_amount", item.Amount!);
                            command.Parameters.AddWithValue("p_paymentdate", item.PaymentDate!);
                            command.Parameters.AddWithValue("p_installmentnumber", item.InstallmentNumber!);
                            command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{item.InstallmentNumber}");

                            await command.ExecuteNonQueryAsync();

                            command.CommandText = $"fetch all in \"ref{item.InstallmentNumber}\"";
                            command.CommandType = CommandType.Text;

                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    dtoInstallmentList.Add(new DTOInstallment
                                    {
                                        Id = (int)reader["Id"],
                                        CreditCardNo = (string)reader["CreditCardNo"],
                                        InstallmentNumber = (int)reader["InstallmentNumber"],
                                        PaymentDate = (DateTime)reader["PaymentDate"],
                                        Success = (bool)reader["Success"],
                                        Amount = (decimal)reader["Amount"],
                                        RecordDate = (DateTime)reader["RecordDate"],
                                        RecordScreen = (string)reader["RecordScreen"]
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

            return dtoInstallmentList;
        }

    }
}
