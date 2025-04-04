using System.Data;

namespace BankingApp.Common.Interfaces;

public interface IUnitOfWork: IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    
    Guid TransactionId { get; }
    void Commit();
    void Rollback();

    IDbCommand CreateCommand(string commandText);
}
