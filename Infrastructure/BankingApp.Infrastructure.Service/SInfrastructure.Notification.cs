using System.Net;
using System.Net.Mail;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Customer.Common.DataTransferObjects;
using BankingApp.Customer.Common.Interfaces;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Infrastructure.Service;

public partial class SInfrastructure : ISInfrastructure
{
    public async Task<MessageContainer> SendMail(MessageContainer requestMessage)
    {
        DTOMail email = requestMessage.Get<DTOMail>()!;
        
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("ozzgur.parbnk@gmail.com",
                Environment.GetEnvironmentVariable("MAIL_PASSWORD")),
            EnableSsl = true
        };

        try
        {
            foreach (string to in email.To!)
            {
                MailMessage mailMessage = new MailMessage("ozzgur.parbnk@gmail.com", to, email.Subject, email.Body);
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
        }
        catch (Exception)
        {
        }

        return new MessageContainer();
    }

    public async Task<MessageContainer> SendNotification(MessageContainer requestMessage)
    {
        List<Notification> notifications = requestMessage.Get<List<Notification>>()!;
        List<DTOLogin> customers = requestMessage.Get<List<DTOLogin>>()!;
            
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
                { Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_ADMIN_JSON")) });
        }

        List<Message> messageList = new List<Message>();

        MessageContainer requestNotificationToken = new MessageContainer(requestMessage.UnitOfWork!);
        DTOLogin? cst;
        for (int i = 0; i < notifications.Count; i++)
        {
            requestNotificationToken.Clear();
            requestNotificationToken.Add(customers[i]);

            MessageContainer response;
            using (var proxy = _serviceProvider.GetRequiredService<ISCustomer>())
            {
                response = await proxy.GetNotificationToken(requestNotificationToken);
            }
            cst = response.Get<DTOLogin>();

            if (cst != null &&
                !string.IsNullOrWhiteSpace(cst.NotificationToken))
            {
                messageList.Add(new Message
                {
                    Notification = notifications[i],
                    Token = cst.NotificationToken
                });
            }
        }

        if (messageList.Count > 0)
        {
            await FirebaseMessaging.DefaultInstance.SendEachAsync(messageList);
        }

        return new MessageContainer();
    }
}