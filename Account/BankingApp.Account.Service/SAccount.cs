using BankingApp.Account.Common.Interfaces;

namespace BankingApp.Account.Service;

public partial class SAccount(IServiceProvider _serviceProvider): ISAccount
{
    public void Dispose()
    {}
}