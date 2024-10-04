using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using Xunit;

namespace BankingApp.Test
{
    public partial class Test
    {
        [Theory]
        [InlineData(new int[] {1}, new string[] {"11111111112"}, new string[] {"3000.0"}, new string[] {"TL"})]
        public async void ExecuteTransferScheduleTest(int[] senderAccountIds, string[] recipientAccountNos, string[] amountsAsString, string[] currencies)
        {
            decimal[] amounts = new decimal[amountsAsString.Length];
            for (int i = 0; i < amountsAsString.Length; i++)    
            {
                amounts[i] = decimal.Parse(amountsAsString[i]);
            }

            MessageContainer requestMessage = new MessageContainer();
            DTOTransfer dtoTransfer;
            for (int i = 0; i < senderAccountIds.Length; i++)
            {
                dtoTransfer = new DTOTransfer{SenderAccountId = senderAccountIds[i], RecipientAccountNo = recipientAccountNos[i], Amount = amounts[i], Currency = currencies[i], OrderDate = DateTime.Today};

                requestMessage.Clear();
                requestMessage.Add(dtoTransfer);
                await _proxy.StartTransfer(requestMessage);
            }

            MessageContainer responseMessage = await _proxy.ExecuteTransferSchedule(new MessageContainer());

            List<DTOTransfer> successTransfers = responseMessage.Get<List<DTOTransfer>>("SuccessTransfers");
            List<DTOTransfer> failedTransfers = responseMessage.Get<List<DTOTransfer>>("FailedTransfers");
            List<DTOTransactionHistory> senderTransactions = responseMessage.Get<List<DTOTransactionHistory>>("SenderTransactions");
            List<DTOTransactionHistory> recipientTransactions = responseMessage.Get<List<DTOTransactionHistory>>("RecipientTransactions");

            for (int i = 0; i < senderAccountIds.Length; i++)
            {
                
            }

            Assert.True(true);
        }
    }
}