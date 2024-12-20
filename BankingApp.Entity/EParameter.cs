using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Threading.Tasks;
using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using System.Data;
using BankingApp.Common.Interfaces;

namespace BankingApp.Entity
{
    public class EParameter(IUnitOfWork unitOfWork)
    {
        public async Task<List<DTOParameter>> GetByMultipleGroupCode(DTOParameter parGroupCodes)
        {
            long now = DateTime.Now.Ticks;
            List<DTOParameter> parameterList = new List<DTOParameter>();
            using (var command =
                   (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT l_parametersbygroupcodes(@refcursor, @p_groupcodes)"))
            {
                command.Parameters.AddWithValue("p_groupcodes", parGroupCodes.GroupCode!);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, $"ref{now}");
                await command.ExecuteNonQueryAsync();

                command.CommandText = $"fetch all in \"ref{now}\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        DTOParameter parameter = new DTOParameter
                        {
                            Id = reader.GetInt32(0),
                            GroupCode = reader.GetString(1),
                            Code = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            Detail1 = await reader.IsDBNullAsync(4) ? null : reader.GetString(4),
                            Detail2 = await reader.IsDBNullAsync(5) ? null : reader.GetString(5),
                            Detail3 = await reader.IsDBNullAsync(6) ? null : reader.GetString(6),
                            Detail4 = await reader.IsDBNullAsync(7) ? null : reader.GetString(7),
                            Detail5 = await reader.IsDBNullAsync(8) ? null : reader.GetString(8)
                        };
                        parameterList.Add(parameter);
                    }
                }
            }

            return parameterList;
        }

        public async Task<DTOParameter> Select(DTOParameter parGroupCodes)
        {
            DTOParameter dtoparameter = new DTOParameter();

            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT s_parameterbygroupcodeandcode(@refcursor, @p_groupcode, @p_code, @p_description)"))
            {
                command.Parameters.AddWithValue("p_groupcode", parGroupCodes.GroupCode!);
                command.Parameters.AddWithValue("p_code", parGroupCodes.Code ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_description", parGroupCodes.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dtoparameter = new DTOParameter
                        {
                            Id = reader.GetInt32(0),
                            GroupCode = reader.GetString(1),
                            Code = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            Detail1 = await reader.IsDBNullAsync(4) ? null : reader.GetString(4),
                            Detail2 = await reader.IsDBNullAsync(5) ? null : reader.GetString(5),
                            Detail3 = await reader.IsDBNullAsync(6) ? null : reader.GetString(6),
                            Detail4 = await reader.IsDBNullAsync(7) ? null : reader.GetString(7),
                            Detail5 = await reader.IsDBNullAsync(8) ? null : reader.GetString(8)
                        };
                    }
                }
            }

            return dtoparameter;
        }

        public async Task<DTOParameter> Update(DTOParameter dtoParameter)
        {
            using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                       "SELECT u_parameter(@refcursor, @p_recorddate, @p_recordscreen, @p_id, @p_groupcode, @p_code, @p_description, @p_detail1, @p_detail2, @p_detail3, @p_detail4, @p_detail5)"))
            {
                command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor, "ref");
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", dtoParameter.RecordScreen);
                command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                command.Parameters.AddWithValue("p_recordscreen", dtoParameter.RecordScreen);
                command.Parameters.AddWithValue("p_id", dtoParameter.Id!);
                command.Parameters.AddWithValue("p_groupcode", dtoParameter.GroupCode!);
                command.Parameters.AddWithValue("p_code", dtoParameter.Code!);
                command.Parameters.AddWithValue("p_description", dtoParameter.Description!);
                command.Parameters.AddWithValue("p_detail1", dtoParameter.Detail1 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_detail2", dtoParameter.Detail2 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_detail3", dtoParameter.Detail3 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_detail4", dtoParameter.Detail4 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_detail5", dtoParameter.Detail5 ?? (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();

                command.CommandText = "fetch all in \"ref\"";
                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dtoParameter = new DTOParameter
                        {
                            Id = reader.GetInt32(0),
                            GroupCode = reader.GetString(1),
                            Code = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            Detail1 = await reader.IsDBNullAsync(4) ? null : reader.GetString(4),
                            Detail2 = await reader.IsDBNullAsync(5) ? null : reader.GetString(5),
                            Detail3 = await reader.IsDBNullAsync(6) ? null : reader.GetString(6),
                            Detail4 = await reader.IsDBNullAsync(7) ? null : reader.GetString(7),
                            Detail5 = await reader.IsDBNullAsync(8) ? null : reader.GetString(8)
                        };
                    }
                }
            }

            return dtoParameter;
        }

        public async Task<List<DTOParameter>> UpdateRange(List<DTOParameter> dtoParameterList)
        {
            long now = DateTime.UtcNow.Ticks;
            List<DTOParameter> parlist = new List<DTOParameter>();
            foreach (var dtoParameter in dtoParameterList)
            {
                using (var command = (NpgsqlCommand)unitOfWork.CreateCommand(
                           "SELECT u_parameter(@refcursor, @p_recorddate, @p_recordscreen, @p_id, @p_groupcode, @p_code, @p_description, @p_detail1, @p_detail2, @p_detail3, @p_detail4, @p_detail5)"))
                {
                    command.Parameters.AddWithValue("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor,
                        $"ref{dtoParameter.Code}{now}");
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", dtoParameter.RecordScreen);
                    command.Parameters.AddWithValue("p_recorddate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("p_recordscreen", dtoParameter.RecordScreen);
                    command.Parameters.AddWithValue("p_id", dtoParameter.Id!);
                    command.Parameters.AddWithValue("p_groupcode", dtoParameter.GroupCode!);
                    command.Parameters.AddWithValue("p_code", dtoParameter.Code!);
                    command.Parameters.AddWithValue("p_description", dtoParameter.Description!);
                    command.Parameters.AddWithValue("p_detail1", dtoParameter.Detail1 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_detail2", dtoParameter.Detail2 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_detail3", dtoParameter.Detail3 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_detail4", dtoParameter.Detail4 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("p_detail5", dtoParameter.Detail5 ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"fetch all in \"ref{dtoParameter.Code}{now}\"";
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            parlist.Add(new DTOParameter
                            {
                                Id = reader.GetInt32(0),
                                GroupCode = reader.GetString(1),
                                Code = reader.GetInt32(2),
                                Description = reader.GetString(3),
                                Detail1 = await reader.IsDBNullAsync(4) ? null : reader.GetString(4),
                                Detail2 = await reader.IsDBNullAsync(5) ? null : reader.GetString(5),
                                Detail3 = await reader.IsDBNullAsync(6) ? null : reader.GetString(6),
                                Detail4 = await reader.IsDBNullAsync(7) ? null : reader.GetString(7),
                                Detail5 = await reader.IsDBNullAsync(8) ? null : reader.GetString(8)
                            });
                        }
                    }
                }
            }

            return parlist;
        }
    }
}