using System.Data;
using BankingApp.Common;
using BankingApp.Infrastructure.Common.Interfaces;
using Npgsql;

namespace BankingApp.Common.DataTransferObjects;

public class UnitOfWork : IUnitOfWork
{
    private readonly NpgsqlConnection _connection;
    private NpgsqlTransaction _transaction;
    private bool _disposed;

    public IDbConnection Connection => _connection;
    public IDbTransaction Transaction => _transaction;
    
    public Guid TransactionId { get; private set; }

    public UnitOfWork()
    {
        _connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
        _connection.Open();
        _transaction = _connection.BeginTransaction();
        TransactionId = Guid.NewGuid();
    }

    public void Commit()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
            Dispose();
        }
    }

    public void Rollback()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public IDbCommand CreateCommand(string sqlQuery)
    {
        var command = (NpgsqlCommand)Connection.CreateCommand();
        command.Transaction = (NpgsqlTransaction)Transaction;
        command.CommandText = sqlQuery;
        return command;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            _connection.Close();
            _connection?.Dispose();
            _disposed = true;
        }
    }
}