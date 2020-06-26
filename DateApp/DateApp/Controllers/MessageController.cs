﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class MessageController : Controller
    {

        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private int MessagePerPage { get; set; }

        public MessageController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
            this.MessagePerPage = 5;
        }
                


        public MessageViewModel SettingMessageView(string ActionMade, string ActivePage, string ReceiverId, string SenderId, SearchDetails Details, bool setLastPage)
        {
            MessageViewModel messageView = new MessageViewModel();

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

            if (setLastPage)
            {
                messageView.info.CurrentPage = messageView.info.TotalPages;
            }


            return messageView;
        }
                      


        public PartialViewResult SelectPage(string ActionMade, string ActivePage, string ReceiverId)
        {
            SearchDetails Details = repository.GetUserDetails(ReceiverId);
            string SenderId = userManager.GetUserId(HttpContext.User);
            MessageViewModel messageView = SettingMessageView(ActionMade, ActivePage, ReceiverId, SenderId, Details, false);

            return PartialView("WriteMessage", messageView);
        }



        [HttpPost]
        public PartialViewResult WriteMessage(string ReceiverId)
        {
            SearchDetails Details = repository.GetUserDetails(ReceiverId);
            string SenderId = userManager.GetUserId(HttpContext.User);

            MessageViewModel messageView = SettingMessageView("None","None", ReceiverId, SenderId, Details, true);

            return PartialView("WriteMessage", messageView);
        }

        [HttpPost]
        public PartialViewResult SendMessage(Message message)
        {
            bool check = repository.SendMessage(message.SenderId, message.ReceiverId, message.MessageText);
            SearchDetails Details = repository.GetUserDetails(message.ReceiverId);
            string SenderId = userManager.GetUserId(HttpContext.User);

            MessageViewModel messageView = SettingMessageView("None", "None", message.ReceiverId, SenderId, Details, true);

            return PartialView("WriteMessage", messageView);
        }


        public IActionResult MessageStart(string UserId)
        {
            string Id = userManager.GetUserId(HttpContext.User);
            bool check = repository.StartChat(Id, UserId);
            return RedirectToRoute(new { controller = "Pair", action = "PairPanel", select = "Pair" });
        }
    }
}