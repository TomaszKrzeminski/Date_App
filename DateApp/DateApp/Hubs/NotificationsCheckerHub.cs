using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Hubs
{
    [Authorize]
    public class NotificationsCheckerHub : Hub
    {

        private IHubContext<NotificationsCheckerHub> notificationcheckerContext;


        public NotificationsCheckerHub(IHubContext<NotificationsCheckerHub> notificationcheckerContext)
        {

            this.notificationcheckerContext = notificationcheckerContext;
        }             



        public Task Notify_CheckAllNotifications(string UserId)
        {
            return Clients.User(UserId).SendAsync("CheckAllNotifications");
        }



    }
}
