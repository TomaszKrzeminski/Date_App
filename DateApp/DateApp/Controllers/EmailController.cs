using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace DateApp.Controllers
{
    public class EmailController : Controller
    {
        private IHostingEnvironment _env;
        private IRepository repository;

        public EmailController(IHostingEnvironment env,IRepository repo)
        {
            _env = env;
            repository = repo;
        }









        public void Send2()
        {

            INotificationEmail EmailPair = new PairNotificationEmail(_env, "zdalnerepo1985@gmail.com","Testowy@gmail.com",DateTime.Now,9,"test.jpg" ,"PairImage.jpg");

            EmailPair.SendEmail();

           

        }

        public void Send3()
        {

            INotificationEmail EmailPair = new MessageNotificationEmail(_env, "zdalnerepo1985@gmail.com", "Testowy@gmail.com", DateTime.Now, 111, "test.jpg", "MessagePage.jpg");

            EmailPair.SendEmail();

           

        }

        public void Send4()
        {

            INotificationEmail EmailPair = new LikeNotificationEmail(_env, "zdalnerepo1985@gmail.com", "Testowy@gmail.com", DateTime.Now, 111, null, "LikePage.jpg");

            EmailPair.SendEmail();

           

        }


        public void Send5()
        {

            INotificationEmail EmailPair = new SuperLikeNotificationEmail(_env, "zdalnerepo1985@gmail.com", "Testowy@gmail.com", DateTime.Now, 111, null, "SuperLikePage.jpg");

            EmailPair.SendEmail();



        }


        public IActionResult Test()
        {

            

            while (repository.GetUserToNotify()!=null)
            {

                string UserId = repository.GetUserToNotify();

                INotificationEmail pair=repository.CheckPairsForNofification( UserId);
                INotificationEmail message= repository.CheckMessagesForNofification( UserId);
                INotificationEmail like= repository.CheckLikesForNotification( UserId);
                INotificationEmail superlike= repository.CheckSuperLikesForNofification( UserId);

                List<INotificationEmail> list = new List<INotificationEmail>() { pair, message, like, superlike };

                foreach (var email in list)
                {

                    if(email!=null)
                    {
                        email.SendEmail();
                    }

                }

                repository.SetNotify(UserId);


            }




            return View();
        }



      
























    }
}