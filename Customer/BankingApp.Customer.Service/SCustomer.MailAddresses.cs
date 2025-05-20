using BankingApp.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Customer.Entity;
using BankingApp.Customer.Common.Interfaces;
using BankingApp.Customer.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Customer.Service
{
    public partial class SCustomer : ISCustomer
    {
        public async Task<MessageContainer> AddMailAddressWithTemporaryPassword(MessageContainer reqMessage)
        {
            EMailAddresses eMailAddress = new EMailAddresses(reqMessage.UnitOfWork);
            DTOCustomer dtoCustomer = reqMessage.Get<DTOCustomer>("Customer");
            DTOMailAddresses dtoMailAddress = reqMessage.Get<DTOMailAddresses>("MailAddress");
            DTOLogin dtoLogin = reqMessage.Get<DTOLogin>("Login");

            await eMailAddress.Add(dtoMailAddress);
            sendMail(new List<string> { dtoMailAddress.MailAddress! }, "ParBank Geçici Parola",
                $"Merhaba {dtoCustomer.Name},<br><br>Bankamıza hoşgeldin. ParBank uygulamasına ilk girişinde kullanabileceğin geçici parola: <strong>{dtoLogin.Password}</strong><br><br>İyi Günler Dileriz.");

            return new MessageContainer();
        }

        public async Task<MessageContainer> SelectMailAddressByMailAddress(MessageContainer requestMessage)
        {
            EMailAddresses eMailAddress = new EMailAddresses(requestMessage.UnitOfWork);
            DTOMailAddresses dtoMailAddress = requestMessage.Get<DTOMailAddresses>();

            MessageContainer reqService = new MessageContainer();
            reqService.Add(await eMailAddress.Get(dtoMailAddress));

            return reqService;
        }

        public async Task<MessageContainer> GetPrimaryMailAddressByCustomerNo(MessageContainer requestMessage)
        {
            EMailAddresses eMailAddress = new EMailAddresses(requestMessage.UnitOfWork);
            DTOMailAddresses dtoMailAddress = requestMessage.Get<DTOMailAddresses>();

            MessageContainer reqService = new MessageContainer();
            reqService.Add(await eMailAddress.Get(dtoMailAddress));

            return reqService;
        }

        private void sendMail(List<string> toList, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("ozzgur.parbnk@gmail.com",
                    Environment.GetEnvironmentVariable("MAIL_PASSWORD")),
                EnableSsl = true
            };

            try
            {
                foreach (string to in toList)
                {
                    MailMessage mailMessage = new MailMessage("ozzgur.parbnk@gmail.com", to, subject, body);
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}