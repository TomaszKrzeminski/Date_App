using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DateApp.Hubs;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DateApp.Controllers
{
    public class MessageController : Controller
    {

        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private int MessagePerPage { get; set; }
        private IHubContext<MessageHub> messageContext;
        private IHubContext<CheckConnectionHub> connectionContext;
        private Func<Task<AppUser>> GetUser;

        public MessageController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env,IHubContext<MessageHub> messageContext,IHubContext<CheckConnectionHub>connectionContext, Func<Task<AppUser>> GetUser = null)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
            this.MessagePerPage = 5;
            this.messageContext = messageContext;
            this.connectionContext = connectionContext;

            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }



        }
                


        public MessageViewModel SettingMessageView(string ActionMade, string ActivePage, string ReceiverId, string SenderId, SearchDetails Details, bool setLastPage)
        {
            MessageViewModel messageView = new MessageViewModel();
            messageView.ReceiverId = ReceiverId;

            List<Message> MessagesList = repository.GetChat(SenderId, ReceiverId).OrderBy(x => x.Time).ToList();
            int MessagesCount = MessagesList.Count;

            if (setLastPage)
            {
                messageView.info = new PageInfo(MessagesCount, ReceiverId, 5);
            }
            else
            {
                messageView.info = new PageInfo(MessagesCount, ReceiverId, ActionMade, ActivePage, MessagePerPage);
            }

            int Page = messageView.info.CurrentPage;
            messageView.conversation = MessagesList.Skip((Page - 1) * MessagePerPage).Take(MessagePerPage).ToList();
            messageView.message = new Message(ReceiverId, SenderId);
            messageView.ReceiverName = Details.User.UserName;
            messageView.ReceiverPhotoPath = Details.MainPhotoPath;
            messageView.UserId = SenderId;
            messageView.UserName = Details.User.Email;
            if (setLastPage)
            {
                messageView.info.CurrentPage = messageView.info.TotalPages;
            }
            return messageView;
        }                    
        
        public PartialViewResult SelectPage(string ActionMade, string ActivePage, string ReceiverId)
        {
            SearchDetails Details = repository.GetUserDetails(ReceiverId);
            string SenderId = GetUser().Result.Id;
            MessageViewModel messageView = SettingMessageView(ActionMade, ActivePage, ReceiverId, SenderId, Details, false);

            return PartialView("WriteMessage", messageView);
        }


        [HttpPost]
        public PartialViewResult RefreshReceivers(string ReceiverId)
        {
            string Id = GetUser().Result.Id;
            SearchDetails details = repository.GetUserDetails(Id);
            AppUser user = repository.GetUser(Id);           
            List<Message> listOfMessages = repository.GetAllMessages(Id);
            listOfMessages = listOfMessages.OrderByDescending(x => x.Time).ToList();
            ///// Remove Repetings
            listOfMessages = listOfMessages.Where(y => y.SenderId != Id).GroupBy(x => new { x.SenderId, x.ReceiverId }).Select(y => y.First()).ToList();
            /////

            List<MessageShort> shortList = new List<MessageShort>();

            foreach (var m in listOfMessages)
            {             

                SearchDetails Details = repository.GetUserDetails(m.SenderId);
                string PhotoPath = Details.MainPhotoPath;
                // check sender/receiver of message and then set name
                string Name = Details.User.UserName;
                ////
                string Text = m.MessageText;
                string ShortText = "";
                bool Checked = m.Checked;
                if (Text != null && Text.Count() > 0)
                {
                    ShortText = Regex.Replace(Text.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                    ShortText += "...";
                }
                shortList.Add(new MessageShort(PhotoPath, ShortText, Name, Details.User.Id, Checked));
            }      
            

            /////
           
            ///


            return PartialView("RefreshReceivers", shortList);         
            
        }

        
        public void CheckOffline()
        {

            string SenderId = GetUser().Result.Id;
            connectionContext.Clients.All.SendAsync("UpdateChatList_Remove", SenderId);


        }       

       [HttpPost]
        public void CheckOnline([FromBody]OnlineObject  data)
        {
            string SenderId = GetUser().Result.Id;
            connectionContext.Clients.User(data.UserId).SendAsync("UpdateChatList_Add", SenderId);
            connectionContext.Clients.User(SenderId).SendAsync("UpdateChatList_Add", data.UserId);

        }



        [HttpPost]
        public PartialViewResult WriteMessage(string ReceiverId)
        {
            SearchDetails Details = repository.GetUserDetails(ReceiverId);
            string SenderId = GetUser().Result.Id;

            MessageViewModel messageView = SettingMessageView("None","None", ReceiverId, SenderId, Details, true);

            //change message to checked

            bool check = repository.ChangeMessagesToRead(SenderId, ReceiverId);


            return PartialView("WriteMessage", messageView);
        }


        [HttpPost]
        public PartialViewResult SendMessage(Message message)
        {
            bool check = repository.SendMessage(message.SenderId, message.ReceiverId, message.MessageText);
            if(check)
            {

                string ReceiverId= message.ReceiverId;
                messageContext.Clients.User(ReceiverId).SendAsync("UpdateChat_Users");
                AppUser user = repository.GetUser(message.SenderId);
                messageContext.Clients.User(ReceiverId).SendAsync("UpdateChat_WriteMessage",user.Email);

            }
            SearchDetails Details = repository.GetUserDetails(message.ReceiverId);
            string SenderId = GetUser().Result.Id;

            MessageViewModel messageView = SettingMessageView("None", "None", message.ReceiverId, SenderId, Details, true);

            return PartialView("WriteMessage", messageView);
        }


        public IActionResult MessageStart(string UserId)
        {
            string Id = GetUser().Result.Id;
            bool check = repository.StartChat(Id, UserId);
            return RedirectToRoute(new { controller = "Pair", action = "PairPanel", select = "Pair" });
        }


       

    }
}