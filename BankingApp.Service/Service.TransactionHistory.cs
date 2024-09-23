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
        public async Task<MessageContainer> GetHistoryByFilter(MessageContainer requestMessage)
        {
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            DTOTransactionHistory dtoTransaction = requestMessage.Get<DTOTransactionHistory>();
            MessageContainer responseMessage = new MessageContainer();

            try
            {
                List<DTOTransactionHistory> transactionList = Mapper.Map<List<DTOTransactionHistory>>(await eTransactionHistory.GetAllByCustomerNoAsync(Mapper.Map<TransactionHistory>(dtoTransaction)));


            transactionList = transactionList.OrderByDescending(x => x.TransactionDate).ToList();

            if(dtoTransaction.Count != null && dtoTransaction.Count != 0 && transactionList.Count() >= dtoTransaction.Count){
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
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseMessage;
        }
    }
}
