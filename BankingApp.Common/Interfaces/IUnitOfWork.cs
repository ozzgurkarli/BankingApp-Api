using System.Data;

namespace BankingApp.Common.Interfaces;

public interface IUnitOfWork
{
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    void Commit();
    void Rollback();
    
    IDbCommand CreateCommand(string commandText);
}
