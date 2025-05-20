using BankingApp.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BankingApp.Account.Common.DataTransferObjects;
using BankingApp.Account.Common.Interfaces;
using BankingApp.Account.Entity;
using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Account.Service
{
    public partial class SAccount : ISAccount
    {
        public async Task<MessageContainer> AddNewTransaction(MessageContainer requestMessage)
        {
            ETransactionHistory eTransactionHistory = new ETransactionHistory(requestMessage.UnitOfWork);
            DTOTransactionHistory dtoTransactionHistory = requestMessage.Get<DTOTransactionHistory>();
            MessageContainer responseMessage = new MessageContainer();

            responseMessage.Add(await eTransactionHistory.Add(dtoTransactionHistory));

            return responseMessage;
        }

        public async Task<MessageContainer> AddMultipleTransactions(MessageContainer requestMessage)
        {
            ETransactionHistory eTransactionHistory = new ETransactionHistory(requestMessage.UnitOfWork);
            List<DTOTransactionHistory> dtoTransactionHistoryList = requestMessage.Get<List<DTOTransactionHistory>>();
            MessageContainer responseMessage = new MessageContainer();

            await eTransactionHistory.AddRange(dtoTransactionHistoryList);

            return responseMessage;
        }

        public async Task<MessageContainer> GetHistoryByFilter(MessageContainer requestMessage)
        {
            ETransactionHistory eTransactionHistory = new ETransactionHistory(requestMessage.UnitOfWork);
            DTOTransactionHistory dtoTransaction = requestMessage.Get<DTOTransactionHistory>();
            MessageContainer responseMessage = new MessageContainer();

            List<DTOTransactionHistory> transactionList = await eTransactionHistory.Get(dtoTransaction);


            transactionList = transactionList.OrderByDescending(x => x.TransactionDate).ToList();

            responseMessage.Add("TransactionList", transactionList);

            return responseMessage;
        }
    }
}