using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.enums;
using BankingApp.Entity;

namespace BankingApp.Service
{
    public partial class Service
    {
        public async Task<MessageContainer> StartTransfer(MessageContainer requestMessage)
        {
            ETransfer eTransfer = new ETransfer();
            ECustomer eCustomer = new ECustomer();
            EAccount eAccount = new EAccount();
            DTOTransfer dtoTransfer = requestMessage.Get<DTOTransfer>();
            DTOAccount? dtoRecipientAcc = null;

            if (dtoTransfer.RecipientAccountNo!.Length > 16)
            {
                dtoTransfer.RecipientAccountNo = dtoTransfer.RecipientAccountNo.Replace(" ", "").Substring(10);
                dtoRecipientAcc = (await eAccount.Get(new DTOAccount { AccountNo = dtoTransfer.RecipientAccountNo })).First(); ;

                if (dtoTransfer.RecipientAccountNo.Substring(2, 2) == "11" && dtoRecipientAcc == null)
                { // if parbank and null
                    throw new Exception("Alıcı bilgileri bulunamadı.");
                }
            }
            else
            {  //identity no
                dtoRecipientAcc = await eAccount.GetByCustomerIdentityNo(new DTOAccount{CustomerIdentityNo = dtoTransfer.RecipientAccountNo, Primary = true});
            }



            dtoTransfer.RecipientAccountNo = dtoRecipientAcc == null ? "0000000000000000" : dtoRecipientAcc.AccountNo;

            dtoTransfer.TransactionDate = DateTime.Now;
            dtoTransfer.Status = (int?)TransferStatus.Waiting;

            Task<DTOTransfer> item = eTransfer.Add(dtoTransfer);

            if (dtoTransfer.OrderDate == DateTime.Today)
            {
                await item;
                Task<MessageContainer> execute = ExecuteTransferSchedule(new MessageContainer());
            }

            return new MessageContainer();
        }

        public async Task<MessageContainer> ExecuteTransferSchedule(MessageContainer requestMessage)
        {
            ETransfer eTransfer = new ETransfer();
            EAccount eAccount = new EAccount();
            ETransactionHistory eTransactionHistory = new ETransactionHistory();
            ECustomer eCustomer = new ECustomer();
            MessageContainer responseMessage = new MessageContainer();
            List<DTOTransactionHistory> senderTransactions = new List<DTOTransactionHistory>();
            List<DTOTransactionHistory> recipientTransactions = new List<DTOTransactionHistory>();
            List<DTOTransfer> successTransfers = new List<DTOTransfer>();
            List<DTOTransfer> failedTransfers = new List<DTOTransfer>();

            List<DTOTransfer> transfers = await eTransfer.GetOrdersToExecute(new DTOTransfer());

            foreach (DTOTransfer transfer in transfers)
            {
                if (transfer.SenderAccountBalance < transfer.Amount || (bool)!transfer.SenderCustomerActive! || (bool)!transfer.RecipientCustomerActive! || !(bool)transfer.SenderAccountActive! || !(bool)transfer.RecipientAccountActive!)
                {
                    transfer.Status = (int?)TransferStatus.Failed;
                    continue;
                }

                transfer.Status = (int?)TransferStatus.Success;
            }

            transfers.ForEach(async x =>
            {
                if (x.Status == (int)TransferStatus.Success)
                {
                    List<DTOAccount> accList = [];
                    List<DTOTransactionHistory> tHistoryList = [];

                    DTOAccount senderAccount = (await eAccount.Get(new DTOAccount { AccountNo = x.SenderAccountNo })).First();
                    DTOAccount recipientAccount = (await eAccount.Get(new DTOAccount { AccountNo = x.RecipientAccountNo })).First();
                    senderAccount.Balance -= x.Amount;
                    recipientAccount.Balance += x.Amount;
                    accList.Add(senderAccount);
                    accList.Add(recipientAccount);

                    tHistoryList.Add(new DTOTransactionHistory { Currency = x.Currency!, CustomerNo = x.SenderCustomerNo, TransactionType = (int)TransactionType.Transfer, AccountNo = x.SenderAccountNo, Amount = -x.Amount, TransactionDate = DateTime.UtcNow });

                    if (x.RecipientAccountNo != "0000000000000000")
                    {
                        tHistoryList.Add(new DTOTransactionHistory { Currency = x.Currency!, CustomerNo = x.RecipientCustomerNo, TransactionType = (int)TransactionType.Transfer, AccountNo = x.RecipientAccountNo, Amount = x.Amount, TransactionDate = DateTime.UtcNow });
                    }

                    await eTransfer.ExecuteTransfer(x, accList, tHistoryList);
                    sendMail([x.SenderMailAddress], "Para Transferi Başarılı", $"Merhaba {x.SenderName},<br><br>Gerçekleştirdiğin para transferi tamamlandı.<br><br>İşlem Tutarı: {x.Amount}<br>Döviz Cinsi: {x.Currency}<br><br>İyi Günler Dileriz.");
                    if (x.RecipientAccountNo != "0000000000000000")
                    {
                        sendMail([x.RecipientMailAddress], "Hesabınıza Para Geldi", $"Merhaba {x.RecipientName},<br><br>{x.SenderName} tarafından size para gönderildi.<br><br>İşlem Tutarı: {x.Amount}<br>Döviz Cinsi: {x.Currency}<br><br>İyi Günler Dileriz.");
                    }


                }
                else if (x.Status == (int)TransferStatus.Failed)
                {
                    failedTransfers.Add(x);
                }
            });

            responseMessage.Add("SuccessTransfers", successTransfers);
            responseMessage.Add("FailedTransfers", failedTransfers);
            responseMessage.Add("SenderTransactions", senderTransactions);
            responseMessage.Add("RecipientTransactions", recipientTransactions);

            return responseMessage;
        }
    }
}