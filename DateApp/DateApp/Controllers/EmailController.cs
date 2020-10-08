using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using DateApp.Jobs;
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
        IScheduler _scheduler;

        public EmailController(IHostingEnvironment env, IRepository repo, IScheduler scheduler)
        {
            _env = env;
            repository = repo;
            _scheduler = scheduler;
        }



        public IActionResult SchedulerDetails()
        {
            return View();
        }

        public async Task<IActionResult> SchedulerAction(string Action)
        {

            IJobDetail job = null;
            string Name = "NotifyWithEmailJob";
            string Group = "Notification";
            JobKey key = new JobKey(Name, Group);


            if (Action == "Start")
            {
                if (_scheduler.CheckExists(key).Result == false)
                {

                    job = JobBuilder.Create<NotificationJob>().WithIdentity(Name, Group).StoreDurably().RequestRecovery().Build();

                    await _scheduler.AddJob(job, true);

                    ITrigger trigger = TriggerBuilder.Create()
                                                    .ForJob(job)
                                                    .WithIdentity("NotifyWithEmail", "Notification")
                                                    .StartNow()
                                                    .WithSimpleSchedule(z => z.WithIntervalInSeconds(120).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
                                                    .Build();

                    await _scheduler.ScheduleJob(trigger);
                }
                

            }
            else if (Action == "Stop")
            {
                if (_scheduler.CheckExists(key).Result)
                {
                    await _scheduler.PauseJob(job.Key);
                }
            }



            return View("SchedulerDetails");
        }

        public IActionResult SchedulerIntervalChange()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {


            IJobDetail job = JobBuilder.Create<NotificationJob>().WithIdentity("notifyjob", "Notification").StoreDurably().RequestRecovery().Build();

            await _scheduler.AddJob(job, true);

            ITrigger trigger = TriggerBuilder.Create()
                                            .ForJob(job)
                                            .WithIdentity("notifytrigger", "Notification")
                                            .StartNow()
                                            .WithSimpleSchedule(z => z.WithIntervalInSeconds(120).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
                                            .Build();

            await _scheduler.ScheduleJob(trigger);



            return View();
        }


        //public async Task<IActionResult> Test2()
        //{       




        //IJobDetail job = JobBuilder.Create<SimpleJob>()
        //                           .UsingJobData("username", "devhow")
        //                           .UsingJobData("password", "Security!!")
        //                           .WithIdentity("simplejob", "quartzexamples")
        //                           .StoreDurably()
        //                           .RequestRecovery()
        //                           .Build();
        //job.JobDataMap.Put("user", new JobUserParameter { Username = "devhow", Password = "Security!!" });
        //await _scheduler.AddJob(job, true);
        ////save the job


        //ITrigger trigger = TriggerBuilder.Create()
        //                                 .ForJob(job)
        //                                 .UsingJobData("triggerparam", "Simple trigger 1 Parameter")
        //                                 .WithIdentity("testtrigger", "quartzexamples")
        //                                 .StartNow()
        //                                 .WithSimpleSchedule(z => z.WithIntervalInSeconds(5).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
        //                                 .Build();
        //ITrigger trigger2 = TriggerBuilder.Create()
        //                                .ForJob(job)
        //                                .UsingJobData("triggerparam", "Simple trigger 2 Parameter")
        //                                .WithIdentity("testtrigger2", "quartzexamples")
        //                                .StartNow()
        //                                .WithSimpleSchedule(z => z.WithIntervalInSeconds(5).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
        //                                .Build();
        //ITrigger trigger3 = TriggerBuilder.Create()
        //                                .ForJob(job)
        //                                .UsingJobData("triggerparam", "Simple trigger 3 Parameter")
        //                                .WithIdentity("testtrigger3", "quartzexamples")
        //                                .StartNow()
        //                                .WithSimpleSchedule(z => z.WithIntervalInSeconds(5).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
        //                                .Build();

        //await _scheduler.ScheduleJob(trigger);
        //await _scheduler.ScheduleJob(trigger2);
        //await _scheduler.ScheduleJob(trigger3);


        //    return View();
        //}



    }
}