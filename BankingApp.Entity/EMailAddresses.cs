using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity
{
    public class EMailAddresses
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task Add(DTOMailAddresses item)
        {
            using (var connection = new NpgsqlConnection(ENV.DatabaseConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("CALL i_mailaddress(@p_recorddate, @p_recordscreen, @p_customerid, @p_mailaddress, @p_primary)", connection))
                {
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", item.RecordScreen);
                    command.Parameters.AddWithValue("p_customerid", Int64.Parse(item.CustomerNo!));
                    command.Parameters.AddWithValue("p_mailaddress", item.MailAddress!);
                    command.Parameters.AddWithValue("p_primary", item.Primary!);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public Task<MailAddresses> GetByMailAddress(MailAddresses item)
        {
            return database.MailAddress.FirstOrDefaultAsync(x => x.MailAddress.Equals(item.MailAddress));
        }

        public async Task<MailAddresses> GetPrimaryAddressByCustomerNo(MailAddresses item)
        {
            return await database.MailAddress.FirstOrDefaultAsync(x => x.Customer.Id.Equals(item.Customer.Id) && x.Primary);
        }
    }
}
