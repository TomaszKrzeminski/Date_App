using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Hubs
{
    [Authorize]
    public class NotificationHub:Hub
    {

        private IHubContext<NotificationHub> notificationContext;


        public NotificationHub(IHubContext<NotificationHub> notificationContext)
        {

            this.notificationContext = notificationContext;
        }


        public Task Notify_NewPair(string UserId)
        {
            return Clients.User(UserId).SendAsync("NotifyNewPair");
        }

        public Task Notify_NewMessage(string UserId)
        {
            return Clients.User(UserId).SendAsync("UpdateChatOptions");
        }

        public Task Notify_NewLikes(string UserId)
        {
            return Clients.User(UserId).SendAsync("UpdateChatOptions");
        }

        public Task Notify_NewSuperLikes(string UserId)
        {
            return Clients.User(UserId).SendAsync("UpdateChatOptions");
        }


    }
}
