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
            DTOAccount dtoRecipientAcc;

            if (dtoTransfer.RecipientAccount.Length > 16)
            {
                dtoTransfer.RecipientAccount = dtoTransfer.RecipientAccount.Replace(" ", "").Substring(10);
            }

            dtoRecipientAcc = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = dtoTransfer.RecipientAccount }));

            dtoTransfer.RecipientAccountId = dtoRecipientAcc.Id;

            dtoTransfer.TransactionDate = DateTime.Now;
            dtoTransfer.Status = (int?)TransferStatus.Waiting;

            Transfer x = new Transfer { SenderAccount = new Account { Id = (int)dtoTransfer.SenderAccountId }, Amount = dtoTransfer.Amount, Currency = dtoTransfer.Currency, OrderDate = dtoTransfer.OrderDate, RecipientAccount = new Account { Id = (int)dtoTransfer.RecipientAccountId }, Status = dtoTransfer.Status, TransactionDate = dtoTransfer.TransactionDate };


            Task<Transfer> item = eTransfer.Add(x);

            if (dtoTransfer.OrderDate == DateTime.Today)
            {
                await item;
                ExecuteTransferSchedule(new MessageContainer());
            }

            return new MessageContainer();
        }

        public async Task<MessageContainer> ExecuteTransferSchedule(MessageContainer requestMessage)
        {
            ETransfer eTransfer = new ETransfer();
            EAccount eAccount = new EAccount();
            ECustomer eCustomer = new ECustomer();

            List<DTOTransfer> transfers = Mapper.Map<List<DTOTransfer>>(await eTransfer.GetTodayOrders(new Transfer()));
            DTOAccount senderAccount, recipientAccount;

            foreach (DTOTransfer transfer in transfers)
            {
                try
                {
                    senderAccount = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = transfer.SenderAccount }));

                    if (senderAccount.Balance < transfer.Amount)
                    {  // insufficent
                        transfer.Status = (int?)TransferStatus.Failed;
                        continue;
                    }

                    recipientAccount = Mapper.Map<DTOAccount>(await eAccount.Get(new Account { AccountNo = transfer.RecipientAccount }));
                    senderAccount.Balance -= transfer.Amount;
                    recipientAccount.Balance += transfer.Amount;

                    await eAccount.Update(Mapper.Map<Account>(senderAccount));
                    await eAccount.Update(Mapper.Map<Account>(recipientAccount));

                    transfer.Status = (int?)TransferStatus.Success;
                }
                catch (Exception)
                {
                    transfer.Status = (int?)TransferStatus.Failed;
                }
            }

            transfers.ForEach(async x =>
            {
                eTransfer.Update(new Transfer { Id = (int)x.Id, SenderAccount = new Account { Id = (int)x.SenderAccountId }, Amount = x.Amount, Currency = x.Currency, OrderDate = x.OrderDate, RecipientAccount = new Account { Id = (int)x.RecipientAccountId }, Status = x.Status, TransactionDate = x.TransactionDate });

                DTOAccount dtoSenderAcc = Mapper.Map<DTOAccount>(await eAccount.Get(new Account{AccountNo = x.SenderAccount}));

                DTOCustomer dtoCustomer = Mapper.Map<DTOCustomer>(await eCustomer.Get(new Customer { Id = int.Parse(dtoSenderAcc.CustomerNo) }));
                if (x.Status == (int)TransferStatus.Success)
                {
                    sendMail([dtoCustomer.PrimaryMailAddress], "Para Transferi Başarılı", $"Merhaba {dtoCustomer.Name},<br><br>Gerçekleştirdiğin para transferi tamamlandı.<br<br>İşlem Tutarı: {x.Amount}<br>Döviz Cinsi: {x.Currency}<br><br>İyi Günler Dileriz.");

                }else if (x.Status == (int)TransferStatus.Failed)
                {
                    sendMail([dtoCustomer.PrimaryMailAddress], "Para Transferi Başarısız", $"Merhaba {dtoCustomer.Name},<br><br>Gerçekleştirdiğin para transferi tamamlanamadı. Detaylı bilgi için müşteri hizmetlerimizle iletişime geçebilirsin.<br<br>İyi Günler Dileriz.");

                }
            });



            return new MessageContainer();
        }
    }
}