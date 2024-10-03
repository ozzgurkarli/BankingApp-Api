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

        public async Task Add(DTOAccount item)
        {
            using(var connection = new NpgsqlConnection(ENV.DatabaseConnectionString)){
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("CALL i_account(@p_recorddate, @p_recordscreen, @p_customerid, @p_accountno, @p_branch, @p_balance, @p_currency, @p_active, @p_primary)", connection)){
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                    command.Parameters.AddWithValue("p_accountno", item.AccountNo!);
                    command.Parameters.AddWithValue("p_branch", item.Branch!);
                    command.Parameters.AddWithValue("p_balance", 0.0M);
                    command.Parameters.AddWithValue("p_currency", item.Currency!);
                    command.Parameters.AddWithValue("p_active", true);
                    command.Parameters.AddWithValue("p_primary", item.Primary!);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
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
