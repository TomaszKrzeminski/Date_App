using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DateApp.Hubs
{

    [Authorize]
    public class MessageHub:Hub
    {


        private IHubContext<MessageHub> messageContext;

        public MessageHub( IHubContext<MessageHub> messageContext)
        {
            
            this.messageContext = messageContext;
        }






        public void Announce(string message)
        {
            Clients.All.SendAsync("Announce",message);
        }



        public Task SendMessage(string UserId,string UserName,string Message)
        {
            
            return Clients.User(UserId).SendAsync("ChatReceive",Message);
        }



        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }


    }


    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    public class EmailBasedUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }

}
