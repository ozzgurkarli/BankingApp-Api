using BankingApp.Customer.Common.Interfaces;

namespace BankingApp.Customer.Service;

public partial class SCustomer(IServiceProvider _serviceProvider) : ISCustomer
{
    public void Dispose()
    { }
}