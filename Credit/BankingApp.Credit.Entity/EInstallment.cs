using BankingApp.Common.DataTransferObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.Interfaces;
using BankingApp.Credit.Common.DataTransferObjects;

namespace BankingApp.Credit.Entity
{
    public class EInstallment(IUnitOfWork _unitOfWork)
    {
        public async Task<List<DTOInstallment>> AddRange(List<DTOInstallment> installmentList)
        {
            List<DTOInstallment> dtoInstallmentList = new List<DTOInstallment>();
            foreach (var item in installmentList)
            {
                using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                           "SELECT i_installment(@refcursor, @p_recorddate, @p_recordscreen, @p_cardno, @p_amount, @p_paymentdate, @p_installmentnumber, @p_transactionid)"))
                {
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_cardno", item.CreditCardNo!);
                    command.Parameters.AddWithValue("p_amount", item.Amount!);
                    command.Parameters.AddWithValue("p_paymentdate", item.PaymentDate!);
                    command.Parameters.AddWithValue("p_installmentnumber", item.InstallmentNumber!);
                    command.Parameters.AddWithValue("p_transactionid", item.TransactionId!);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                        $"ref{item.InstallmentNumber}");

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
                                RecordScreen = (string)reader["RecordScreen"],
                                TransactionId = (int)reader["TransactionId"]
                            });
                        }
                    }
                }
            }

            return dtoInstallmentList;
        }

        public async Task<List<DTOInstallment>> UpdateRange(List<DTOInstallment> installmentList)
        {
            List<DTOInstallment> dtoInstallmentList = new List<DTOInstallment>();
            int count = 0;
            long now = DateTime.UtcNow.Ticks;
            foreach (var item in installmentList)
            {
                count++;
                using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                           "SELECT u_installment(@refcursor, @p_id, @p_recorddate, @p_recordscreen, @p_cardno, @p_amount, @p_success, @p_paymentdate, @p_installmentnumber)"))
                {
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_cardno", item.CreditCardNo!);
                    command.Parameters.AddWithValue("p_amount", item.Amount!);
                    command.Parameters.AddWithValue("p_id", item.Id!);
                    command.Parameters.AddWithValue("p_success", item.Success!);
                    command.Parameters.AddWithValue("p_paymentdate", item.PaymentDate!);
                    command.Parameters.AddWithValue("p_installmentnumber", item.InstallmentNumber!);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                        $"ref{count}{now}");

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"fetch all in \"ref{count}{now}\"";
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
                                RecordScreen = (string)reader["RecordScreen"],
                                TransactionId = (int)reader["TransactionId"]
                            });
                        }
                    }
                }
            }

            return dtoInstallmentList;
        }

        public async Task<List<DTOInstallment>> GetInstallmentsToExecute(DTOInstallment item)
        {
            List<DTOInstallment> installmentList = new List<DTOInstallment>();
            long now = DateTime.UtcNow.Ticks;
            using (var command =
                   (NpgsqlCommand)_unitOfWork.CreateCommand(
                       "SELECT l_installmentstoexecute(@refcursor, @p_orderdate)"))
            {
                command.Parameters.AddWithValue("p_orderdate", DateTime.Today);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");
                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        installmentList.Add(new DTOInstallment
                        {
                            Id = (int)reader["Id"],
                            CreditCardNo = (string)reader["CreditCardNo"],
                            Success = (bool)reader["Success"],
                            Amount = (decimal)reader["Amount"],
                            PaymentDate = (DateTime)reader["PaymentDate"],
                            InstallmentNumber = (int)reader["InstallmentNumber"],
                            TransactionCompany = await reader.IsDBNullAsync("TransactionCompany")
                                ? null
                                : (string)reader["TransactionCompany"]
                        });
                    }
                }
            }

            return installmentList;
        }
    }
}