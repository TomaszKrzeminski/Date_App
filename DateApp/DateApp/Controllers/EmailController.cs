using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

using DateApp.Models;
using DateApp.Models.DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Quartz;

namespace DateApp.Controllers
{
    public class EmailController : Controller
    {
        private IHostingEnvironment _env;
        private IRepository repository;
        private IScheduler scheduler;

        public EmailController(IHostingEnvironment env, IRepository repo, IScheduler scheduler)
        {
            _env = env;
            repository = repo;
            this.scheduler = scheduler;
        }

      

        public async Task<IActionResult> Test()
        {

            

            return View();
        }



        public void Send()
        {
            while (repository.GetUserToNotify() != null)
            {

                string UserId = repository.GetUserToNotify();

                NotificationEmail pair = repository.CheckPairsForNofification(UserId);
                NotificationEmail message = repository.CheckMessagesForNofification(UserId);
                NotificationEmail like = repository.CheckLikesForNotification(UserId);
                NotificationEmail superLike = repository.CheckSuperLikesForNofification(UserId);

                List<NotificationEmail> list = new List<NotificationEmail>() { pair, message ,like,superLike};

                foreach (var email in list)
                {

                    if (email != null)
                    {
                        email.MakeEmail();
                        email.SendEmail();
                    }

                }

                repository.SetNotify(UserId);


            }




        }


        //public IActionResult Test()
        //{



        //    while (repository.GetUserToNotify() != null)
        //    {

        //        string UserId = repository.GetUserToNotify();

        //        INotificationEmail pair = repository.CheckPairsForNofification(UserId);
        //        INotificationEmail message = repository.CheckMessagesForNofification(UserId);
        //        INotificationEmail like = repository.CheckLikesForNotification(UserId);
        //        INotificationEmail superlike = repository.CheckSuperLikesForNofification(UserId);

        //        List<INotificationEmail> list = new List<INotificationEmail>() { pair, message, like, superlike };

        //        foreach (var email in list)
        //        {

        //            if (email != null)
        //            {
        //                email.SendEmail();
        //            }

        //        }

        //        repository.SetNotify(UserId);


        //    }




        //    return View();
        //}




























    }
}