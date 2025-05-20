using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Infrastructure.Common.DataTransferObjects;
using BankingApp.Infrastructure.Common.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace BankingApp.Infrastructure.Service;

public partial class SInfrastructure : ISInfrastructure
{

    public Task<MessageContainer> SendNotification(MessageContainer requestMessage)
    {
        List<Notification> notifications = requestMessage.Get<List<Notification>>()!;
        List<DTOLogin> 
            
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
                { Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_ADMIN_JSON")) });
        }

        List<Message> messageList = new List<Message>();

        MessageContainer requestNotificationToken = new MessageContainer(unitOfWork);
        DTOLogin? cst;
        for (int i = 0; i < notifications.Count; i++)
        {
            requestNotificationToken.Clear();
            requestNotificationToken.Add(customers[i]);
            cst = (await GetNotificationToken(requestNotificationToken)).Get<DTOLogin>();

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

        return true;
    }
    private async Task<bool> sendNotification(List<Notification> notifications, List<DTOLogin> customers,
        IUnitOfWork unitOfWork)
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
                { Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_ADMIN_JSON")) });
        }

        List<Message> messageList = new List<Message>();

        MessageContainer requestNotificationToken = new MessageContainer(unitOfWork);
        DTOLogin? cst;
        for (int i = 0; i < notifications.Count; i++)
        {
            requestNotificationToken.Clear();
            requestNotificationToken.Add(customers[i]);
            cst = (await GetNotificationToken(requestNotificationToken)).Get<DTOLogin>();

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

        return true;
    }
}