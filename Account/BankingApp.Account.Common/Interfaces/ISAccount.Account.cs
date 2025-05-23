﻿using BankingApp.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Account.Common.Interfaces
{
    public partial interface ISAccount
    {
        public Task<MessageContainer> CreateAccount(MessageContainer requestMessage);
        
        public Task<MessageContainer> UpdateRangeAccount(MessageContainer requestMessage);


        public Task<MessageContainer> GetAccountsByFilter(MessageContainer requestMessage);
    }
}
