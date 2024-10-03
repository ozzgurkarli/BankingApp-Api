using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using BankingApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankingApp.Service
{
    public partial class Service : IService
    {
        public async Task<MessageContainer> AddNewTransaction(MessageContainer requestMessage)
        {
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            DTOTransactionHistory dtoTransactionHistory = requestMessage.Get<DTOTransactionHistory>();
            MessageContainer responseMessage = new MessageContainer();
            
            await eTransactionHistory.Add(dtoTransactionHistory);

            return responseMessage;
        }

        public async Task<MessageContainer> GetHistoryByFilter(MessageContainer requestMessage)
        {
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            DTOTransactionHistory dtoTransaction = requestMessage.Get<DTOTransactionHistory>();
            MessageContainer responseMessage = new MessageContainer();

            List<DTOTransactionHistory> transactionList = await eTransactionHistory.Get(dtoTransaction);


            transactionList = transactionList.OrderByDescending(x => x.TransactionDate).ToList();

            if (dtoTransaction.Count != null && dtoTransaction.Count != 0 && transactionList.Count() >= dtoTransaction.Count)
            {
                transactionList = transactionList.GetRange(0, (int)dtoTransaction.Count);
            }

            if (dtoTransaction.MinDate > DateTime.MinValue)
            {
                transactionList.Where(x => x.TransactionDate >= dtoTransaction.MinDate);
            }

            if (dtoTransaction.MaxDate > DateTime.MinValue)
            {
                transactionList.Where(x => x.TransactionDate <= dtoTransaction.MaxDate);
            }

            responseMessage.Add("TransactionList", transactionList);

            return responseMessage;
        }
    }
}
