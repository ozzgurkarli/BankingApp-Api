using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Account.Common.DataTransferObjects;
using BankingApp.Account.Common.Interfaces;
using BankingApp.Account.Entity;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Account.Common.Enums;
using BankingApp.Customer.Common.DataTransferObjects;
using BankingApp.Customer.Entity;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Account.Service
{
    public partial class SAccount: ISAccount
    {
        public async Task<MessageContainer> CheckRecipientCustomer(MessageContainer requstMessage)
        {
            ECustomer eCustomer = new ECustomer(requstMessage.UnitOfWork);
            DTOTransfer dtoTransfer = requstMessage.Get<DTOTransfer>();
            MessageContainer responseMessage = new MessageContainer();
            DTOCustomer customer = new DTOCustomer();

            if (dtoTransfer.RecipientAccountNo!.Length > 11 && dtoTransfer.RecipientAccountNo.Substring(2, 2) == "11")
            {
                dtoTransfer.RecipientAccountNo = dtoTransfer.RecipientAccountNo.Replace(" ", "").Substring(10);
                customer = await eCustomer.GetByAccountNo(
                    new DTOCustomer { AccountNo = dtoTransfer.RecipientAccountNo });
            }
            else
            {
                customer = await eCustomer.Get(new DTOCustomer { IdentityNo = dtoTransfer.RecipientAccountNo });
            }

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                throw new Exception(
                    "Alıcı bilgileri hatalıdır, devam etmeniz halinde transfer işlemi başarısız olarak tamamlanacak.");
            }
            else if (!(bool)customer.Active!)
            {
                throw new Exception(
                    "Alıcının hesabı aktif değil, devam etmeniz halinde transfer işlemi başarısız olarak tamamlanacak.");
            }

            responseMessage.Add("DTOCustomer", customer);
            return responseMessage;
        }

        public async Task<MessageContainer> StartTransfer(MessageContainer requestMessage)
        {
            ETransfer eTransfer = new ETransfer(requestMessage.UnitOfWork);
            EAccount eAccount = new EAccount(requestMessage.UnitOfWork);
            DTOTransfer dtoTransfer = requestMessage.Get<DTOTransfer>();
            DTOAccount? dtoRecipientAcc = null;

            if (dtoTransfer.RecipientAccountNo!.Length > 16)
            {
                dtoTransfer.RecipientAccountNo = dtoTransfer.RecipientAccountNo.Replace(" ", "").Substring(10);
                dtoRecipientAcc = (await eAccount.Get(new DTOAccount { AccountNo = dtoTransfer.RecipientAccountNo }))
                    .FirstOrDefault();
                ;

                if (dtoTransfer.RecipientAccountNo.Substring(2, 2) == "11" && dtoRecipientAcc == null)
                {
                    // if parbank and null
                    throw new Exception("Alıcı bilgileri bulunamadı.");
                }
            }
            else
            {
                //identity no
                dtoRecipientAcc = await eAccount.GetByCustomerIdentityNo(new DTOAccount
                    { CustomerIdentityNo = dtoTransfer.RecipientAccountNo, Primary = true });
            }


            dtoTransfer.RecipientAccountNo = dtoRecipientAcc == null ? "0000000000000000" : dtoRecipientAcc.AccountNo;

            dtoTransfer.TransactionDate = DateTime.Now;
            dtoTransfer.Status = (int?)TransferStatus.Waiting;

            Task<DTOTransfer> item = eTransfer.Add(dtoTransfer);

            if (dtoTransfer.OrderDate == DateTime.Today)
            {
                await item;
                MessageContainer execute =
                    await ExecuteTransferSchedule(new MessageContainer(requestMessage.UnitOfWork));
            }

            return new MessageContainer();
        }

        public async Task<MessageContainer> ExecuteTransferSchedule(MessageContainer requestMessage)
        {
            ETransfer eTransfer = new ETransfer(requestMessage.UnitOfWork);
            EAccount eAccount = new EAccount(requestMessage.UnitOfWork);
            MessageContainer responseMessage = new MessageContainer();
            List<DTOTransactionHistory> senderTransactions = new List<DTOTransactionHistory>();
            List<DTOTransactionHistory> recipientTransactions = new List<DTOTransactionHistory>();
            List<DTOTransfer> successTransfers = new List<DTOTransfer>();
            List<DTOTransfer> failedTransfers = new List<DTOTransfer>();
            
            List<Notification> notificationList = new List<Notification>();
            List<DTOLogin> notificationUserList = new List<DTOLogin>();

            List<DTOTransfer> transfers = await eTransfer.GetOrdersToExecute(new DTOTransfer());

            foreach (DTOTransfer transfer in transfers)
            {
                if (transfer.SenderAccountBalance < transfer.Amount || (bool)!transfer.SenderCustomerActive! ||
                    !(bool)transfer.SenderAccountActive! ||
                    (transfer.RecipientCustomerActive != null && (bool)!transfer.RecipientCustomerActive!) ||
                    (transfer.RecipientAccountActive != null && !(bool)transfer.RecipientAccountActive!))
                {
                    transfer.Status = (int?)TransferStatus.Failed;
                    continue;
                }

                transfer.Status = (int?)TransferStatus.Success;
            }

            foreach (DTOTransfer x in transfers)
            {
                if (x.Status == (int)TransferStatus.Success)
                {
                    List<DTOAccount> accList = [];
                    List<DTOTransactionHistory> tHistoryList = [];

                    DTOAccount senderAccount =
                        (await eAccount.Get(new DTOAccount { AccountNo = x.SenderAccountNo })).First();
                    DTOAccount recipientAccount =
                        (await eAccount.Get(new DTOAccount { AccountNo = x.RecipientAccountNo })).First();
                    senderAccount.Balance -= x.Amount;
                    recipientAccount.Balance += x.Amount;
                    accList.Add(senderAccount);

                    tHistoryList.Add(new DTOTransactionHistory
                    {
                        Currency = x.Currency!, CustomerNo = x.SenderCustomerNo,
                        TransactionType = (int)TransactionType.Transfer, AccountNo = x.SenderAccountNo,
                        Amount = -x.Amount, TransactionDate = DateTime.UtcNow
                    });

                    if (!string.IsNullOrWhiteSpace(x.RecipientAccountNo) && x.RecipientAccountNo != "0000000000000000")
                    {
                        accList.Add(recipientAccount);
                        tHistoryList.Add(new DTOTransactionHistory
                        {
                            Currency = x.Currency!, CustomerNo = x.RecipientCustomerNo,
                            TransactionType = (int)TransactionType.Transfer, AccountNo = x.RecipientAccountNo,
                            Amount = x.Amount, TransactionDate = DateTime.UtcNow
                        });
                    }

                    requestMessage.Clear();
                    requestMessage.Add(accList);
                    await UpdateRangeAccount(requestMessage);

                    requestMessage.Clear();
                    requestMessage.Add(tHistoryList);
                    await AddMultipleTransactions(requestMessage);

                    await eTransfer.ExecuteTransfer(x);
                    

                    sendMail([x.SenderMailAddress], "Para Transferi Başarılı",
                        $"Merhaba {x.SenderName},<br><br>Gerçekleştirdiğin para transferi tamamlandı.<br><br>İşlem Tutarı: {formatAmount((decimal)x.Amount!)}<br>Döviz Cinsi: {x.Currency}<br><br>İyi Günler Dileriz.");
                    if (!string.IsNullOrWhiteSpace(x.RecipientAccountNo) && x.RecipientAccountNo != "0000000000000000")
                    {
                        notificationList.Add(new Notification{Title = "Parbank", Body = $"{x.SenderName} size {formatAmount((decimal)x.Amount!)} {x.Currency} tutarında para gönderdi."});
                        notificationUserList.Add(new DTOLogin { CustomerNo = x.RecipientCustomerNo });
                        sendMail([x.RecipientMailAddress], "Hesabınıza Para Geldi",
                            $"Merhaba {x.RecipientName},<br><br>{x.SenderName} tarafından size para gönderildi.<br><br>İşlem Tutarı: {formatAmount((decimal)x.Amount!)}<br>Döviz Cinsi: {x.Currency}<br><br>İyi Günler Dileriz.");
                    }
                }
                else if (x.Status == (int)TransferStatus.Failed)
                {
                    failedTransfers.Add(x);
                }
            }

            MessageContainer requestNotification = new MessageContainer(requestMessage.UnitOfWork);
            requestNotification.Add(notificationList);
            requestNotification.Add(notificationUserList);
            
            using (var proxy = _serviceProvider.GetRequiredService<ISInfrastructure>())
            {
                await proxy.SendNotification(requestNotification);
            }
            
            responseMessage.Add("SuccessTransfers", successTransfers);
            responseMessage.Add("FailedTransfers", failedTransfers);
            responseMessage.Add("SenderTransactions", senderTransactions);
            responseMessage.Add("RecipientTransactions", recipientTransactions);

            return responseMessage;
        }

        private string formatAmount(decimal amount)
        {
            CultureInfo cultureInfo = new CultureInfo("tr-TR");
            return amount.ToString("N", cultureInfo);
        }
    }
}