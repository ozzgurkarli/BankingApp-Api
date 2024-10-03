using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
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
    public class EAccount
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<DTOAccount> Add(DTOAccount item)
        {
            DTOAccount dtoAccount = new DTOAccount();
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                using (var command = new NpgsqlCommand("SELECT i_account(@refcursor, @p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_branch, @p_balance, @p_currency, @p_active, @p_primary)", connection, tran))
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
                    await tran.CommitAsync();
                }
                await tran.DisposeAsync();
                await connection.CloseAsync();
            }

            return dtoAccount;
        }

        public async Task<List<DTOAccount>> UpdateRange(List<DTOAccount> accList)
        {
            List<DTOAccount> dtoAccountList = new List<DTOAccount>();
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();

                try
                {
                    foreach (var item in accList)
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

                            command.CommandText = $"fetch all in \"ref{item.AccountNo}\"";
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
                    await tran.CommitAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
                await tran.DisposeAsync();
                await connection.CloseAsync();
            }

            return dtoAccountList;
        }

        public async Task<DTOAccount> GetFirstAvailableNoAndIncrease(DTOAccount acc)
        {
            DTOAccount dtoAccount = new DTOAccount();

            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                NpgsqlTransaction tran = await connection.BeginTransactionAsync();
                using (var command = new NpgsqlCommand("SELECT u_accounttracker(@refcursor, @p_currencycode)", connection))
                {
                    command.Parameters.AddWithValue("p_currencycode", acc.CurrencyCode!);
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

                    await tran.CommitAsync();
                }
                await tran.DisposeAsync();
                await connection.CloseAsync();
            }


            return dtoAccount;
        }


        public async Task<Account> Update(Account item)
        {
            using (var context = new BankingDbContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.Entry(item.Customer).State = EntityState.Unchanged;
                item = (context.Account.Update(item)).Entity;

                await context.SaveChangesAsync();
            }

            return item;
        }
        public async Task<List<Account>> UpdateAll(List<Account> items)
        {
            try
            {

                database.ChangeTracker.AutoDetectChangesEnabled = false;
                database.Account.UpdateRange(items);

                await database.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return items;
        }

        public async Task<Account> Get(Account item)
        {
            return await database.Account.Where(x => x.AccountNo.Equals(item.AccountNo)).Include(x => x.Customer).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> GetAll()
        {
            return await database.Account.Include(x => x.Customer).ToListAsync();
        }
    }
}
