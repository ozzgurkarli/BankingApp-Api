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
        public async Task<MessageContainer> StartTransfer(MessageContainer requestMessage){
            ETransfer eTransfer = new ETransfer();
            ECustomer eCustomer = new ECustomer();
            EAccount eAccount = new EAccount();
            DTOTransfer dtoTransfer = requestMessage.Get<DTOTransfer>();

            if(dtoTransfer.RecipientAccount.Length == 11){      // if got identity no, get accountno
                DTOAccount dtoRecipient = Mapper.Map<DTOAccount>((await eCustomer.GetByIdentityNoIncludeAccounts(new Customer{IdentityNo = dtoTransfer.RecipientAccount})).Accounts.FirstOrDefault(x=> x.Primary.Equals(true)));
                dtoTransfer.RecipientAccount = dtoRecipient.AccountNo;
                dtoTransfer.RecipientCustomerNo = dtoRecipient.CustomerNo;
            }
            else{
                DTOAccount dtoRecipient = Mapper.Map<DTOAccount>(await eAccount.Get(new Account{AccountNo = dtoTransfer.RecipientAccount.Replace(" ", "").Substring(10)}));
                dtoTransfer.RecipientAccount = dtoRecipient.AccountNo;
                dtoTransfer.RecipientCustomerNo = dtoRecipient.CustomerNo;
            }

            dtoTransfer.TransactionDate = DateTime.Now;
            dtoTransfer.Status = (int?)TransferStatus.Waiting;
Task<Transfer> item;    // BURALARI FULL DUZELT AMK BUNLAR NEE
            Transfer x = new Transfer{SenderAccount = new Account{AccountNo = dtoTransfer.SenderAccount, Customer = new Customer{Id = int.Parse(dtoTransfer.SenderCustomerNo)}}, Amount = dtoTransfer.Amount, Currency = dtoTransfer.Currency, OrderDate = dtoTransfer.OrderDate, RecipientAccount = new Account{AccountNo = dtoTransfer.RecipientAccount, Id = 9, Customer = new Customer{Id = int.Parse(dtoTransfer.RecipientCustomerNo)}}, Status = dtoTransfer.Status, TransactionDate = dtoTransfer.TransactionDate};
            x.SenderAccount = new Account{AccountNo = dtoTransfer.SenderAccount, Id = 1, Customer = new Customer{Id = int.Parse(dtoTransfer.SenderCustomerNo)}};
            try{
                item = eTransfer.Add(x);
            }
            catch(Exception e){
                throw e;
            }

            if(dtoTransfer.TransactionDate == DateTime.Today){
                await item;
                ExecuteTransferSchedule(new MessageContainer());
            }

            return new MessageContainer();
        }

        public async Task<MessageContainer> ExecuteTransferSchedule(MessageContainer requestMessage){
            ETransfer eTransfer = new ETransfer();
            EAccount eAccount = new EAccount();

            List<DTOTransfer> transfers = Mapper.Map<List<DTOTransfer>>(await eTransfer.GetTodayOrders(new Transfer()));
            DTOAccount senderAccount, recipientAccount;

            foreach(DTOTransfer transfer in transfers){
                senderAccount = Mapper.Map<DTOAccount>(await eAccount.Get(new Account{AccountNo = transfer.SenderAccount}));

                if(senderAccount.Balance < transfer.Amount){  // insufficent
                    transfer.Status = (int?)TransferStatus.Failed;
                    continue;
                }

                recipientAccount = Mapper.Map<DTOAccount>(await eAccount.Get(new Account{AccountNo = transfer.RecipientAccount}));
                senderAccount.Balance -= transfer.Amount;
                recipientAccount.Balance += transfer.Amount;

                await eAccount.UpdateAll([Mapper.Map<Account>(senderAccount), Mapper.Map<Account>(recipientAccount)]);

                transfer.Status = (int?)TransferStatus.Success;
            }

            eTransfer.UpdateAll(Mapper.Map<List<Transfer>>(transfers));


            return new MessageContainer();
        }
    }
}