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
    //public interface INotificationsSheduler
    //{
    //    IRepository repository { get; set; }
    //    void SendNotification();

    //}



    //public class NotificationsSheduler : INotificationsSheduler
    //{
    //    public IRepository repository { get; set; }

    //    public NotificationsSheduler(IRepository repository)
    //    {
    //        this.repository = repository;
    //    }

    //    public void SendNotification()
    //    {
    //        while (repository.GetUserToNotify() != null)
    //        {
    //            string UserId = repository.GetUserToNotify();

    //            NotificationEmail pair = repository.CheckPairsForNofification(UserId);
    //            NotificationEmail message = repository.CheckMessagesForNofification(UserId);
    //            NotificationEmail like = repository.CheckLikesForNotification(UserId);
    //            NotificationEmail superLike = repository.CheckSuperLikesForNofification(UserId);

    //            List<NotificationEmail> list = new List<NotificationEmail>() { pair, message, like, superLike };

    //            foreach (var email in list)
    //            {
    //                if (email != null)
    //                {
    //                    email.MakeEmail();
    //                    email.SendEmail();
    //                }
    //            }

    //            repository.SetNotify(UserId);
    //        }
    //    }


    //}

    //public class NotificationJob : IJob
    //{
    //    INotificationsSheduler notify;



    //    public NotificationJob(INotificationsSheduler notify,IRepository repository)
    //    {
    //        this.notify = notify;
    //    }
    //    public async Task Execute(IJobExecutionContext context)
    //    {
    //        notify.SendNotification();
    //    }
    //}




    //public interface INotificationsSheduler
    //{
    //    IRepository repository { get; set; }
    //    void SendNotification();

    //}



    //public class NotificationsSheduler : INotificationsSheduler
    //{
    //    public IRepository repository { get; set; }


    //    public NotificationsSheduler(IRepository repository)
    //    {
    //        this.repository = repository;
    //    }

    //    public void SendNotification()
    //    {
    //        while (repository.GetUserToNotify() != null)
    //        {
    //            string UserId = repository.GetUserToNotify();

    //            NotificationEmail pair = repository.CheckPairsForNofification(UserId);
    //            NotificationEmail message = repository.CheckMessagesForNofification(UserId);
    //            NotificationEmail like = repository.CheckLikesForNotification(UserId);
    //            NotificationEmail superLike = repository.CheckSuperLikesForNofification(UserId);

    //            List<NotificationEmail> list = new List<NotificationEmail>() { pair, message, like, superLike };

    //            foreach (var email in list)
    //            {
    //                if (email != null)
    //                {
    //                    email.MakeEmail();
    //                    email.SendEmail();
    //                }
    //            }

    //            repository.SetNotify(UserId);
    //        }
    //    }


    //}

    //public class NotificationJob : IJob
    //{
    //    private readonly IServiceProvider _provider;



    //    public NotificationJob(IServiceProvider _provider)
    //    {
    //        this._provider = _provider;
    //    }
    //    public async Task Execute(IJobExecutionContext context)
    //    {

    //        using (var scope = _provider.CreateScope())
    //        {
    //            var repository = scope.ServiceProvider.GetService<IRepository>();
    //            INotificationsSheduler notify = new NotificationsSheduler(repository);
    //            notify.SendNotification();


    //        }





    //    }
    //}


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

        public NotificationJob(INotificationsSheduler notify)
        {
            this.notify = notify;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            notify.SendNotification();
        }
    }



}
