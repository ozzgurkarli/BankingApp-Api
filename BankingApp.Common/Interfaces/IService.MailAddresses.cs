﻿using BankingApp.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.Interfaces
{
    public partial interface IService
    {
        public Task<MessageContainer> AddMailAddressWithTemporaryPassword(MessageContainer reqMessage);

        public Task<MessageContainer> SelectMailAddressByMailAddress(MessageContainer requestMessage);

        public Task<MessageContainer> GetPrimaryMailAddressByCustomerNo(MessageContainer requestMessage);
    }
}
