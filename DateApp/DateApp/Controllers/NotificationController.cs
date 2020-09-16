using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class NotificationController : Controller
    {

      public IRepository repository;

        public NotificationController(IRepository repo)
        {
            repository = repo;
        }


       
        public IActionResult CheckNotifty(string Id)
        {


            NotificationViewModel model = repository.GetNotifications(Id);



            return View(model);
        }
    }
}