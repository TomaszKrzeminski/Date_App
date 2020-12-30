using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Hubs
{

    public class VideoConnectionHub:Hub
    {


        public Task RedirectToVideoCallSender(string SenderId,string ReceiverId)
        {

            return Clients.User(SenderId).SendAsync("redirectToVideoCallSender", ReceiverId);

        }


        public Task AskForCall(string SenderId, string ReceiverId)
        {
            return Clients.User(ReceiverId).SendAsync("askForCall", SenderId);
        }

        public Task AsnwerCall(string SenderId, string ReceiverId)
        {
            return Clients.User(ReceiverId).SendAsync("SendPeerId", SenderId);
        }


        public Task GetPeerId(string SenderId, string ReceiverId)
        {
            return Clients.User(ReceiverId).SendAsync("SendPeerId", SenderId);
        }

        public Task SendPeerId(string SenderId, string PeerId)
        {
            return Clients.User(SenderId).SendAsync("Get_ReceiverId", PeerId);
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