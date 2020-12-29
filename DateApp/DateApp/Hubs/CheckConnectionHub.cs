using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Hubs
{

    public class VideoConnectionHub:Hub
    {

        public Task GetPeerId(string SenderId, string ReceiverId)
        {
            return Clients.User(ReceiverId).SendAsync("SendPeerId", SenderId);
        }

        public Task SendPeerId(string SenderId, string PeerId)
        {
            return Clients.User(SenderId).SendAsync("Get_ReceiverId", PeerId);
        }


        public Task Send(string UserId, string SenderId,string Message)
        {

            return Clients.User(UserId).SendAsync("Check", SenderId,Message);



        }
    }






    public class CheckConnectionHub : Hub
    {


        public async void Ask_Chat_Users(List<string> ChatUserList,string UserId)
        {

            foreach (var chatUser in ChatUserList)
            {


        await  Clients.User(chatUser).SendAsync("CheckStatus",UserId);
            }
                                            
        }



       



        public Task Online(string UserId,string SenderId)
        {

return Clients.User(UserId).SendAsync("UpdateChatList_Add", SenderId);
            


        }

        public Task Offline(string UserId, string SenderId)
        {


            return Clients.User(UserId).SendAsync("UpdateChatList_Remove", SenderId);


        }




    }
}