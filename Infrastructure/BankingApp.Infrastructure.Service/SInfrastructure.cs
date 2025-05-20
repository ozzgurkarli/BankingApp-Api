using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;

namespace BankingApp.Infrastructure.Service
{
    public partial class SInfrastructure(IServiceProvider _serviceProvider): ISInfrastructure
    {
        public void Dispose()
        { }
    }
}