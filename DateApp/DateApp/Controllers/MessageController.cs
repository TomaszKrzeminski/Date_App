using System;
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

        public MessageController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
        }




        [HttpPost]
        public PartialViewResult WriteMessage(string ReceiverId)
        {
            SearchDetails Details = repository.GetUserDetails(ReceiverId);
            string SenderId = userManager.GetUserId(HttpContext.User);

            MessageViewModel messageView = new MessageViewModel();


            messageView.conversation = repository.GetChat(SenderId, ReceiverId);
            messageView.message = new Message(ReceiverId, SenderId);
            messageView.ReceiverName = Details.User.UserName;
            messageView.ReceiverPhotoPath = Details.MainPhotoPath;


            return PartialView("WriteMessage",messageView);
        }

        [HttpPost]
        public PartialViewResult SendMessage(Message message)
        {
            bool check = repository.SendMessage(message.SenderId, message.ReceiverId, message.MessageText);

            SearchDetails Details = repository.GetUserDetails(message.ReceiverId);
            string SenderId = userManager.GetUserId(HttpContext.User);

            MessageViewModel messageView = new MessageViewModel();

            messageView.conversation = repository.GetChat(SenderId, message.ReceiverId);
            messageView.message = new Message(message.ReceiverId, SenderId);
            messageView.ReceiverName = Details.User.UserName;
            messageView.ReceiverPhotoPath = Details.MainPhotoPath;

            return PartialView("WriteMessage",messageView);
        }


        public IActionResult MessageStart(string UserId)
        {
            string Id = userManager.GetUserId(HttpContext.User);
            bool check = repository.StartChat(Id, UserId);


            return RedirectToRoute(new { controller = "Pair", action = "PairPanel", select = "Pair" });
        }
    }
}