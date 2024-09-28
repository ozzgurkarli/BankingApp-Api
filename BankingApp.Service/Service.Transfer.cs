using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.enums;
using BankingApp.Entity;
using BankingApp.Entity.Entities;

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

            if (dtoTransfer.RecipientAccount!.Length > 16)
            {
                dtoTransfer.RecipientAccount = dtoTransfer.RecipientAccount.Replace(" ", "").Substring(10);
                dtoRecipientAcc = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = dtoTransfer.RecipientAccount }));

                if (dtoTransfer.RecipientAccount.Substring(2, 2) == "11" && dtoRecipientAcc == null)
                { // if parbank and null
                    throw new Exception("Alıcı bilgileri bulunamadı.");
                }
            }
            else
            {  //identity no
                Customer? dtoCustomer = await eCustomer.GetByIdentityNoIncludeAccounts(new Customer { IdentityNo = dtoTransfer.RecipientAccount });

                if (dtoCustomer != null && dtoCustomer.Accounts != null)
                {
                    dtoRecipientAcc = Mapper.Map<DTOAccount>(dtoCustomer.Accounts.FirstOrDefault(x => x.Primary == true));
                }
            }



            dtoTransfer.RecipientAccountId = dtoRecipientAcc == null ? 6 : dtoRecipientAcc.Id;

            dtoTransfer.TransactionDate = DateTime.Now;
            dtoTransfer.Status = (int?)TransferStatus.Waiting;

            Transfer x = new Transfer { SenderAccount = new Account { Id = (int)dtoTransfer.SenderAccountId! }, Amount = dtoTransfer.Amount, Currency = dtoTransfer.Currency, OrderDate = dtoTransfer.OrderDate, RecipientAccount = new Account { Id = (int)dtoTransfer.RecipientAccountId! }, Status = dtoTransfer.Status, TransactionDate = dtoTransfer.TransactionDate };


            Task<Transfer> item = eTransfer.Add(x);

            if (dtoTransfer.OrderDate == DateTime.Today)
            {
                await item;
                Task<MessageContainer> execute = ExecuteTransferSchedule(new MessageContainer());

                if (dtoTransfer.TestData != null && (bool)dtoTransfer.TestData)
                {     // test data expects executetransferschedule
                    return await execute;
                }
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
            Task task;

            List<DTOTransfer> transfers = Mapper.Map<List<DTOTransfer>>(await eTransfer.GetTodayOrders(new Transfer()));
            DTOAccount senderAccount, recipientAccount;

            foreach (DTOTransfer transfer in transfers)
            {
                try
                {
                    senderAccount = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = transfer.SenderAccount }));
                    recipientAccount = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = transfer.RecipientAccount }));

                    if (senderAccount == null || recipientAccount == null || senderAccount.Balance < transfer.Amount || (senderAccount.CustomerActive != null && (bool)!senderAccount.CustomerActive) || (recipientAccount.CustomerActive != null && (bool)!recipientAccount.CustomerActive) || !(bool)senderAccount.Active! || !(bool)recipientAccount.Active!)
                    {
                        transfer.Status = (int?)TransferStatus.Failed;
                        continue;
                    }

                    senderAccount.Balance -= transfer.Amount;
                    recipientAccount.Balance += transfer.Amount;

                    task = eAccount.Update(Mapper.Map<Account>(senderAccount));
                    task = eAccount.Update(Mapper.Map<Account>(recipientAccount));

                    transfer.Status = (int?)TransferStatus.Success;
                }
                catch (Exception)
                {
                    transfer.Status = (int?)TransferStatus.Failed;
                }
            }

            transfers.ForEach(async x =>
            {
                successTransfers.Add(Mapper.Map<DTOTransfer>(await eTransfer.Update(new Transfer { Id = (int)x.Id!, SenderAccount = new Account { Id = (int)x.SenderAccountId! }, Amount = x.Amount, Currency = x.Currency, OrderDate = x.OrderDate, RecipientAccount = new Account { Id = (int)x.RecipientAccountId! }, Status = x.Status, TransactionDate = x.TransactionDate })));

                DTOAccount dtoSenderAcc = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = x.SenderAccount }));
                DTOCustomer dtoCustomer = Mapper.Map<DTOCustomer>(await eCustomer.GetIncludeMailAddress(new Customer { Id = int.Parse(dtoSenderAcc.CustomerNo!) }));
                if (x.Status == (int)TransferStatus.Success)
                {
                    DTOAccount? dtoRecipientAcc = null;
                    DTOCustomer? dtoRecipientCustomer = null;

                    if (x.RecipientAccountId != 6)
                    {
                        dtoRecipientAcc = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = x.RecipientAccount }));
                        dtoRecipientCustomer = Mapper.Map<DTOCustomer>(await eCustomer.GetIncludeMailAddress(new Customer { Id = int.Parse(dtoRecipientAcc.CustomerNo!) }));
                    }

                    MessageContainer requestTransaction = new MessageContainer();
                    MessageContainer responseTransaction = new MessageContainer();
                    requestTransaction.Add(new DTOTransactionHistory { Currency = x.Currency!, CustomerNo = dtoCustomer.CustomerNo, TransactionType = (int)TransactionType.Transfer, AccountNo = dtoSenderAcc.AccountNo, AccountId = dtoSenderAcc.Id, Amount = -x.Amount, TransactionDate = DateTime.UtcNow });
                    responseTransaction = await AddNewTransaction(requestTransaction);
                    senderTransactions.Add(responseTransaction.Get<DTOTransactionHistory>());
                    sendMail([dtoCustomer.PrimaryMailAddress], "Para Transferi Başarılı", $"Merhaba {dtoCustomer.Name},<br><br>Gerçekleştirdiğin para transferi tamamlandı.<br><br>İşlem Tutarı: {x.Amount}<br>Döviz Cinsi: {x.Currency}<br><br>İyi Günler Dileriz.");

                    if (x.RecipientAccountId != 6 && dtoRecipientCustomer != null && dtoRecipientAcc != null)
                    {
                        requestTransaction.Clear();
                        requestTransaction.Add(new DTOTransactionHistory { Currency = x.Currency!, CustomerNo = dtoRecipientCustomer.CustomerNo, TransactionType = (int)TransactionType.Transfer, AccountNo = dtoRecipientAcc.AccountNo, AccountId = dtoRecipientAcc.Id, Amount = x.Amount, TransactionDate = DateTime.UtcNow });
                        responseTransaction = await AddNewTransaction(requestTransaction);
                        recipientTransactions.Add(responseTransaction.Get<DTOTransactionHistory>());
                        sendMail([dtoRecipientCustomer.PrimaryMailAddress], "Hesabınıza Para Geldi", $"Merhaba {dtoRecipientCustomer.Name},<br><br>{dtoCustomer.Name} tarafından size para gönderildi.<br><br>İşlem Tutarı: {x.Amount}<br>Döviz Cinsi: {x.Currency}<br><br>İyi Günler Dileriz.");
                    }

                }
                else if (x.Status == (int)TransferStatus.Failed)
                {
                    failedTransfers.Add(x);
                    sendMail([dtoCustomer.PrimaryMailAddress], "Para Transferi Başarısız", $"Merhaba {dtoCustomer.Name},<br><br>Gerçekleştirdiğin para transferi tamamlanamadı. Detaylı bilgi için müşteri hizmetlerimizle iletişime geçebilirsin.<br<br>İyi Günler Dileriz.");

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