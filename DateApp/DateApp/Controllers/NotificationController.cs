using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class NotificationController : Controller
    {


        private IRepository repository;       
        private Func<Task<AppUser>> GetUser;
        private UserManager<AppUser> userManager;

        public NotificationController(IRepository repo, UserManager<AppUser> userMgr, Func<Task<AppUser>> GetUser = null)
        {
            repository = repo;
            userManager = userMgr;

            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }

        }
                                 



        //public IRepository repository;

        //public NotificationController(IRepository repo)
        //{
        //    repository = repo;
        //}

        public IActionResult GetNotifty()
        {
            string Id = GetUser().Result.Id;
            NotificationViewModel model = repository.GetNotifications(Id);
            int count = repository.PotentialMatches(Id);
            model.PotentialMatches = count;
            return View("CheckNotifty", model);
        }

        public IActionResult CheckNotifty(string Id)
        {

           
            NotificationViewModel model = repository.GetNotifications(Id);
            int count = repository.PotentialMatches(Id);
            model.PotentialMatches = count;


            return View(model);
        }
    }
}