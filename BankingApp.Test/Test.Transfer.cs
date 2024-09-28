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
        [InlineData(1, "11111111112", 30000, "TL")]
        public async void ExecuteTransferScheduleTest(int senderAccountId, string recipientAccountNo, decimal amount, string currency)
        {
            // BURASI PARAMETRE OLARAK LISTE ALIP, FOR ICINDE START TRANSFER CALISTIRIP, EN SON EXECUTESCHEDULE ILE TUM VERILERI ALACAK SEKILDE DUZENLENEBILIR???
            MessageContainer requestMessage = new MessageContainer();
            DTOTransfer dtoTransfer = new DTOTransfer{SenderAccountId = senderAccountId, RecipientAccount = recipientAccountNo, Amount = amount, Currency = currency, OrderDate = DateTime.Today, TestData = true};

            requestMessage.Add(dtoTransfer);
            MessageContainer responseMessage = await _proxy.StartTransfer(requestMessage);

            List<DTOTransfer> successTransfers = responseMessage.Get<List<DTOTransfer>>("SuccessTransfers");
            List<DTOTransfer> failedTransfers = responseMessage.Get<List<DTOTransfer>>("FailedTransfers");

            Assert.True(true);
        }
    }
}