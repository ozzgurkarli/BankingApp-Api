using BankingApp.Common.DataTransferObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BankingApp.Credit.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;

namespace BankingApp.Credit.Entity
{
    public class ECreditCard(IUnitOfWork _unitOfWork)
    {
        public async Task<List<DTOCreditCard>> Get(DTOCreditCard cc)
        {
            long now = DateTime.Now.Ticks;
            List<DTOCreditCard> ccList = new List<DTOCreditCard>();
            using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                       "SELECT l_creditcard(@refcursor, @p_customerid, @p_expirationdate, @p_billingday, @p_active)"))
            {
                command.Parameters.AddWithValue("p_customerid",
                    cc.CustomerNo != null ? Int64.Parse(cc.CustomerNo) : (object)DBNull.Value);
                command.Parameters.AddWithValue("p_expirationdate", cc.ExpirationDate == null || cc.ExpirationDate == DateTime.MinValue ? (object)DBNull.Value : cc.ExpirationDate);
                command.Parameters.AddWithValue("p_billingday", cc.BillingDay ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_active", cc.Active ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ccList.Add(new DTOCreditCard
                        {
                            Id = (int)reader["Id"],
                            CardNo = (string?)reader["CardNo"],
                            Active = (bool?)reader["Active"],
                            Limit = (decimal?)reader["Limit"],
                            CurrentDebt = (decimal?)reader["CurrentDebt"],
                            CVV = (Int16?)reader["CVV"],
                            ExpirationDate = (DateTime?)reader["ExpirationDate"],
                            BillingDay = (Int16?)reader["BillingDay"],
                            Type = (int?)reader["Type"],
                            TypeName = (string?)reader["TypeName"],
                            OutstandingBalance = (decimal?)reader["OutstandingBalance"],
                            TotalDebt = (decimal?)reader["TotalDebt"],
                            EndOfCycleDebt = (decimal?)reader["EndOfCycleDebt"],
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString()
                        });
                    }
                }
            }

            return ccList;
        }

        public async Task<DTOCreditCard> Select(DTOCreditCard cc)
        {
            using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand("SELECT s_creditcard(@refcursor, @p_cardno)"))
            {
                command.Parameters.AddWithValue("p_cardno", cc.CardNo!);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cc = new DTOCreditCard
                        {
                            Id = (int)reader["Id"],
                            CardNo = (string?)reader["CardNo"],
                            Active = (bool?)reader["Active"],
                            Limit = (decimal?)reader["Limit"],
                            CurrentDebt = (decimal?)reader["CurrentDebt"],
                            CVV = (Int16?)reader["CVV"],
                            ExpirationDate = (DateTime?)reader["ExpirationDate"],
                            BillingDay = (Int16?)reader["BillingDay"],
                            Type = (int?)reader["Type"],
                            TypeName = (string?)reader["TypeName"],
                            OutstandingBalance = (decimal?)reader["OutstandingBalance"],
                            TotalDebt = (decimal?)reader["TotalDebt"],
                            EndOfCycleDebt = (decimal?)reader["EndOfCycleDebt"],
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString()
                        };
                    }
                }
            }

            return cc;
        }

        public async Task<DTOCreditCard> SelectWithDetails(DTOCreditCard cc)
        {
            using (var command =
                   (NpgsqlCommand)_unitOfWork.CreateCommand("SELECT s_creditcardwithdetails(@refcursor, @p_cardno)"))
            {
                command.Parameters.AddWithValue("p_cardno", cc.CardNo!);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cc = new DTOCreditCard
                        {
                            Id = (int)reader["Id"],
                            CardNo = (string?)reader["CardNo"],
                            Active = (bool?)reader["Active"],
                            Limit = (decimal?)reader["Limit"],
                            CurrentDebt = (decimal?)reader["CurrentDebt"],
                            CVV = (Int16?)reader["CVV"],
                            ExpirationDate = (DateTime?)reader["ExpirationDate"],
                            BillingDay = (Int16?)reader["BillingDay"],
                            Type = (int?)reader["Type"],
                            TypeName = (string?)reader["TypeName"],
                            OutstandingBalance = (decimal?)reader["OutstandingBalance"],
                            TotalDebt = (decimal?)reader["TotalDebt"],
                            EndOfCycleDebt = (decimal?)reader["EndOfCycleDebt"],
                            TypeFee = decimal.Parse(((string)reader["TypeFee"]).Replace('.', ',')),
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString()
                        };
                    }
                }
            }

            return cc;
        }

        public async Task<DTOCreditCard> Add(DTOCreditCard cc)
        {
            using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                       "SELECT i_creditcard(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_cardno, @p_cvv, @p_limit, @p_expirationdate, @p_billingday, @p_type, @p_transactionid)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", cc.RecordScreen);
                command.Parameters.AddWithValue("p_customerid", Int64.Parse(cc.CustomerNo!));
                command.Parameters.AddWithValue("p_cardno", cc.CardNo!);
                command.Parameters.AddWithValue("p_cvv", cc.CVV!);
                command.Parameters.AddWithValue("p_limit", cc.Limit!);
                command.Parameters.AddWithValue("p_expirationdate", cc.ExpirationDate!);
                command.Parameters.AddWithValue("p_billingday", cc.BillingDay!);
                command.Parameters.AddWithValue("p_type", cc.Type!);
                command.Parameters.AddWithValue("p_transactionid", _unitOfWork.TransactionId);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");

                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cc = new DTOCreditCard
                        {
                            Id = (int)reader["Id"],
                            CardNo = (string?)reader["CardNo"],
                            Active = (bool?)reader["Active"],
                            Limit = (decimal?)reader["Limit"],
                            CurrentDebt = (decimal?)reader["CurrentDebt"],
                            CVV = (Int16?)reader["CVV"],
                            ExpirationDate = (DateTime?)reader["ExpirationDate"],
                            BillingDay = (Int16?)reader["BillingDay"],
                            Type = (int?)reader["Type"],
                            OutstandingBalance = (decimal?)reader["OutstandingBalance"],
                            TotalDebt = (decimal?)reader["TotalDebt"],
                            EndOfCycleDebt = (decimal?)reader["EndOfCycleDebt"],
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString()
                        };
                    }
                }
            }

            return cc;
        }

        public async Task<DTOCreditCard> Update(DTOCreditCard cc)
        {
            long now = DateTime.UtcNow.Ticks;
            using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                       "SELECT u_creditcard(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_cardno, @p_cvv, @p_limit, @p_expirationdate, @p_billingday, @p_type, @p_currentdebt, @p_outstandingbalance, @p_totaldebt, @p_id, @p_endofcycledebt, @p_transactionid)"))
            {
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", cc.RecordScreen);
                command.Parameters.AddWithValue("p_customerid", Int64.Parse(cc.CustomerNo!));
                command.Parameters.AddWithValue("p_cardno", cc.CardNo!);
                command.Parameters.AddWithValue("p_cvv", cc.CVV!);
                command.Parameters.AddWithValue("p_limit", cc.Limit!);
                command.Parameters.AddWithValue("p_expirationdate", cc.ExpirationDate!);
                command.Parameters.AddWithValue("p_billingday", cc.BillingDay!);
                command.Parameters.AddWithValue("p_type", cc.Type!);
                command.Parameters.AddWithValue("p_currentdebt", cc.CurrentDebt!);
                command.Parameters.AddWithValue("p_outstandingbalance", cc.OutstandingBalance!);
                command.Parameters.AddWithValue("p_totaldebt", cc.TotalDebt!);
                command.Parameters.AddWithValue("p_endofcycledebt", cc.EndOfCycleDebt!);
                command.Parameters.AddWithValue("p_id", cc.Id!);
                command.Parameters.AddWithValue("p_transactionid", _unitOfWork.TransactionId);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");

                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cc = new DTOCreditCard
                        {
                            Id = (int)reader["Id"],
                            CardNo = (string?)reader["CardNo"],
                            Active = (bool?)reader["Active"],
                            Limit = (decimal?)reader["Limit"],
                            CurrentDebt = (decimal?)reader["CurrentDebt"],
                            CVV = (Int16?)reader["CVV"],
                            ExpirationDate = (DateTime?)reader["ExpirationDate"],
                            BillingDay = (Int16?)reader["BillingDay"],
                            Type = (int?)reader["Type"],
                            OutstandingBalance = (decimal?)reader["OutstandingBalance"],
                            TotalDebt = (decimal?)reader["TotalDebt"],
                            EndOfCycleDebt = (decimal?)reader["EndOfCycleDebt"],
                            CustomerNo = ((Int64)reader["CustomerId"]).ToString()
                        };
                    }
                }
            }

            return cc;
        }

        public async Task<List<DTOCreditCard>> UpdateRange(List<DTOCreditCard> ccList)
        {
            List<DTOCreditCard> dtoCCList = new List<DTOCreditCard>();
            int count = 0;
            long now = DateTime.UtcNow.Ticks;
            foreach (var item in ccList)
            {
                count++;
                using (var command = (NpgsqlCommand)_unitOfWork.CreateCommand(
                           "SELECT u_creditcard(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_cardno, @p_cvv, @p_limit, @p_expirationdate, @p_billingday, @p_type, @p_currentdebt, @p_outstandingbalance, @p_totaldebt, @p_id, @p_endofcycledebt, @p_transactionid)"))
                {
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                    command.Parameters.AddWithValue("p_cardno", item.CardNo!);
                    command.Parameters.AddWithValue("p_cvv", item.CVV!);
                    command.Parameters.AddWithValue("p_limit", item.Limit!);
                    command.Parameters.AddWithValue("p_expirationdate", item.ExpirationDate!);
                    command.Parameters.AddWithValue("p_billingday", item.BillingDay!);
                    command.Parameters.AddWithValue("p_type", item.Type!);
                    command.Parameters.AddWithValue("p_currentdebt", item.CurrentDebt!);
                    command.Parameters.AddWithValue("p_outstandingbalance", item.OutstandingBalance!);
                    command.Parameters.AddWithValue("p_totaldebt", item.TotalDebt!);
                    command.Parameters.AddWithValue("p_endofcycledebt", item.EndOfCycleDebt!);
                    command.Parameters.AddWithValue("p_id", item.Id!);
                    command.Parameters.AddWithValue("p_transactionid", _unitOfWork.TransactionId);
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                        $"ref{count}{now}");

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"fetch all in \"ref{count}{now}\"";
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dtoCCList.Add(new DTOCreditCard
                            {
                                Id = (int)reader["Id"],
                                CardNo = (string?)reader["CardNo"],
                                Active = (bool?)reader["Active"],
                                Limit = (decimal?)reader["Limit"],
                                CurrentDebt = (decimal?)reader["CurrentDebt"],
                                CVV = (Int16?)reader["CVV"],
                                ExpirationDate = (DateTime?)reader["ExpirationDate"],
                                BillingDay = (Int16?)reader["BillingDay"],
                                Type = (int?)reader["Type"],
                                OutstandingBalance = (decimal?)reader["OutstandingBalance"],
                                TotalDebt = (decimal?)reader["TotalDebt"],
                                EndOfCycleDebt = (decimal?)reader["EndOfCycleDebt"],
                                CustomerNo = ((Int64)reader["CustomerId"]).ToString()
                            });
                        }
                    }
                }
            }

            return ccList;
        }
    }
}