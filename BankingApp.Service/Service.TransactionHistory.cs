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
    public partial class Service: IService
    {
        public async Task<MessageContainer> GetHistoryByFilter(MessageContainer requestMessage){
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            DTOTransactionHistory dtoTransaction = requestMessage.Get<DTOTransactionHistory>();

            List<DTOTransactionHistory> transactionList = Mapper.Map<List<DTOTransactionHistory>>(eTransactionHistory.GetAllByCustomerNoAsync(Mapper.Map<TransactionHistory>(dtoTransaction)));

            transactionList = transactionList.OrderBy(x=> x.TransactionDate).ToList();

            if(dtoTransaction.MinDate >= DateTime.MinValue){
                transactionList.Where(x=> x.TransactionDate >= dtoTransaction.MinDate);
            }

            if(dtoTransaction.MaxDate >= DateTime.MinValue){
                transactionList.Where(x=> x.TransactionDate <= dtoTransaction.MaxDate);
            }

            MessageContainer responseMessage = new MessageContainer();

            responseMessage.Add(transactionList);

            return responseMessage;
        }
    }
}
