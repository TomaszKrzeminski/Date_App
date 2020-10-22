using DateApp.Models.DateApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
   

    public interface INotificationsSheduler
    {
        IServiceProvider _provider { get; set; }
        void SendNotification();

    }



    public class NotificationsSheduler : INotificationsSheduler
    {

        public IServiceProvider _provider { get; set; }

        public NotificationsSheduler(IServiceProvider _provider)
        {
            this._provider = _provider;
        }

        public void SendNotification()
        {

            using (var scope = _provider.CreateScope())
            {

                IRepository repository = scope.ServiceProvider.GetService<IRepository>();

                while (repository.GetUserToNotify() != null)
                {
                    string UserId = repository.GetUserToNotify();

                    NotificationEmail pair = repository.CheckPairsForNofification(UserId);
                    NotificationEmail message = repository.CheckMessagesForNofification(UserId);
                    NotificationEmail like = repository.CheckLikesForNotification(UserId);
                    NotificationEmail superLike = repository.CheckSuperLikesForNofification(UserId);

                    List<NotificationEmail> list = new List<NotificationEmail>() { pair, message, like, superLike };

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

        }


    }

    public class NotificationJob : IJob
    {
        INotificationsSheduler notify;


        public NotificationJob()
        {

        }


        public NotificationJob(INotificationsSheduler notify)
        {
            this.notify = notify;
        }

       


        public async Task Execute(IJobExecutionContext context)
        {
            notify.SendNotification();
        }
    }


    public class TestJob1Minute : IJob
    {
        

        public TestJob1Minute()
        {
           
        }
        public async Task Execute(IJobExecutionContext context)
        {
            DateTime time = DateTime.Now;
            Debug.WriteLine("TestJob1Minute xxxxxxxxxxxxxxxx execute "+time);
        }
    }


    public class TestJob2Minutes : IJob
    {


        public TestJob2Minutes()
        {

        }
        public async Task Execute(IJobExecutionContext context)
        {
            DateTime time = DateTime.Now;
            Debug.WriteLine("TestJob2Minutes xxxxxxxxxxxxxxxx execute " + time);
        }
    }



}
