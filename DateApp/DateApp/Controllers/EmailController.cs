using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static Quartz.MisfireInstruction;

namespace DateApp.Controllers
{
    public class EmailController : Controller
    {
        private IHostingEnvironment _env;
        private IRepository repository;
        IRepositoryQuartz repositoryQuartz;
        IScheduler _scheduler;

        public EmailController(IHostingEnvironment env, IRepository repo, IScheduler scheduler, IRepositoryQuartz repositoryQuartz)
        {
            _env = env;
            repository = repo;
            _scheduler = scheduler;
            this.repositoryQuartz = repositoryQuartz;
        }


        public async Task<bool> BuidlDefaultJobs()
        {

            var jobs = _scheduler.GetCurrentlyExecutingJobs();

            if (repositoryQuartz.CheckJobCount() <= 0)
            {

                IJobDetail job = null;
                string Name = "NotifyWithEmailJob";
                string Group = "Notification";
                JobKey key = new JobKey(Name, Group);

                job = JobBuilder.Create<NotificationJob>().WithIdentity(Name, Group).StoreDurably().RequestRecovery().Build();

                await _scheduler.AddJob(job, true);

                ITrigger trigger = TriggerBuilder.Create()
                                                .ForJob(job)
                                                .WithIdentity("NotifyWithEmail", "Notification")
                                                .StartNow()
                                                .WithCronSchedule("0/10 * * 1/1 * ? *")
                                                .Build();



                await _scheduler.ScheduleJob(trigger);

                IJobDetail job2 = JobBuilder.Create<TestJob1Minute>().WithIdentity("TestJob1_5min", "Test").StoreDurably().RequestRecovery().Build();

                await _scheduler.AddJob(job2, true);

                ITrigger trigger2 = TriggerBuilder.Create()
                                                .ForJob(job2)
                                                .WithIdentity("Testing", "Test")
                                                .StartNow()
                                                .WithCronSchedule("0 0/5 * 1/1 * ? *")
                                                .Build();


                await _scheduler.ScheduleJob(trigger2);

                IJobDetail job3 = JobBuilder.Create<TestJob2Minutes>().WithIdentity("TestJob2hours", "Test").StoreDurably().RequestRecovery().Build();

                await _scheduler.AddJob(job3, true);

                ITrigger trigger3 = TriggerBuilder.Create()
                                                .ForJob(job3)
                                                .WithIdentity("Testing2", "Test")
                                                .StartNow()
                                                .WithCronSchedule("0 0 14 1/7 * ? *")
                                                .Build();

                await _scheduler.ScheduleJob(trigger3);


                IJobDetail job4 = JobBuilder.Create<TestJob2Minutes>().WithIdentity("TestJob3", "Test").StoreDurably().RequestRecovery().Build();

                await _scheduler.AddJob(job4, true);

                ITrigger trigger4 = TriggerBuilder.Create()
                                                .ForJob(job4)
                                                .WithIdentity("Testing3", "Test")
                                                .StartNow()
                                               .WithCronSchedule("0 5 14 1/2 * ? *")
                                                .Build();

                await _scheduler.ScheduleJob(trigger4);

                IJobDetail job5 = JobBuilder.Create<TestJob2Minutes>().WithIdentity("TestJob5", "Test").StoreDurably().RequestRecovery().Build();

                await _scheduler.AddJob(job5, true);

                ITrigger trigger5 = TriggerBuilder.Create()
                                                .ForJob(job5)
                                                .WithIdentity("Testing5", "Test")
                                                .StartNow()
                                               .WithCronSchedule("0 0/30 0/2 1/1 * ? *")
                                                .Build();

                await _scheduler.ScheduleJob(trigger5);






                IJobDetail job6 = JobBuilder.Create<TestJob2Minutes>().WithIdentity("TestJob10sec", "Test").StoreDurably().RequestRecovery().Build();

                await _scheduler.AddJob(job6, true);

                ITrigger trigger6 = TriggerBuilder.Create()
                                                .ForJob(job6)
                                                .WithIdentity("Testing6", "Test")
                                                .StartNow()
                                               .WithCronSchedule("0/10 * * 1/1 * ? *")
                                                .Build();

                await _scheduler.ScheduleJob(trigger6);


                IJobDetail job7 = JobBuilder.Create<TestJob2Minutes>().WithIdentity("TestJob75min10sec", "Test").StoreDurably().RequestRecovery().Build();

                await _scheduler.AddJob(job7, true);

                ITrigger trigger7 = TriggerBuilder.Create()
                                                .ForJob(job7)
                                                .WithIdentity("Testing6", "Test")
                                                .StartNow()
                                               .WithCronSchedule("10 0/5 * 1/1 * ? *")
                                                .Build();

                await _scheduler.ScheduleJob(trigger7);













                return true;
            }
            else
            {
                return false;
            }





        }


        string[] GetCharacters(string text)
        {
            string[] characters = text.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return characters;
        }

        CroneDate GetCroneDate(string croneText)
        {
            CroneDate date;
            string[] array = GetCharacters(croneText);

            string DateText = array[3].ToString();
            string Date = DateText.Split("/").Last();

            List<int> list = new List<int>();

            int Day = Int32.Parse(Date);

            if (Day > 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    list.Add(Int32.Parse(array[i]));
                }


            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    string data = array[i];
                    int number;
                    if (data == "*")
                    {
                        number = 0;
                    }
                    else
                    {

                        string Number = data.Split("/").Last();

                        number = Int32.Parse(Number);

                    }
                    list.Add(number);

                }


            }

            date = new CroneDate(Day, list[2], list[1], list[0]);


            return date;
        }


        //string MakeNumber(int Number)
        //{
        //    string number;
        //    if (Number == 0)
        //    {
        //        number = "*";
        //    }
        //    else
        //    {
        //        number = "0/" + Number;
        //    }
        //    return number;

        //}

        string MakeCroneDate(CroneDate crone)
        {
            string CroneText = "";
            string Ending = " * ? *";

            if (crone.Days > 1)
            {
                CroneText = crone.Seconds + " " + crone.Minutes + " " + crone.Hours + " 1/" + crone.Days + Ending;

            }
            else
            {

                string Hours = "*";
                string Minutes = "*";
                string Seconds = "*";
                bool check = true;
                bool action = false;


                if (crone.Hours > 0)
                {

                    if (check)
                    {
                        Hours = "0/" + crone.Hours;
                    }
                    else
                    {
                        Hours = crone.Hours.ToString();
                    }

                    check = false;
                    action = true;
                }

                if (crone.Minutes > 0 ||action)
                {

                    if (check)
                    {
                        Minutes = "0/" + crone.Minutes.ToString();
                    }
                    else
                    {
                        Minutes = crone.Minutes.ToString();
                    }

                    check = false;
                    action = true;
                }


                if (crone.Seconds > 0 ||action)
                {

                    if (check)
                    {
                        Seconds = "0/" + crone.Seconds.ToString();
                    }
                    else
                    {
                        Seconds =crone.Seconds.ToString();
                    }


                }

                CroneText = Seconds + " " + Minutes + " " + Hours + " 1/" + crone.Days + Ending;

            }

            return CroneText;
        }


        public async Task<IActionResult> SchedulerDetails()
        {
            SchedulerViewModel model = repositoryQuartz.GetQuartzReport();

            BuidlDefaultJobs();

            return View(model);
        }

        public async Task<IActionResult> ActionAllJobs(string Action)
        {

            if (Action == "Start")
            {

                await _scheduler.ResumeAll();
            }
            else if (Action == "Stop")
            {
                await _scheduler.PauseAll();
            }

            return RedirectToAction("SchedulerDetails");

        }



        public IActionResult EditJob(string JobName, string Group, string TriggerName, string TriggerGroup)
        {
            JobKey key = new JobKey(JobName, Group);
            IJobDetail details = _scheduler.GetJobDetail(key).Result;
            TriggerKey triggerKey = new TriggerKey(TriggerName, TriggerGroup);
            ITrigger trigger = _scheduler.GetTrigger(triggerKey).Result;

            ICronTrigger cronT = (ICronTrigger)trigger;
            string CronExpression = cronT.CronExpressionString;

            CroneDate croneDate = GetCroneDate(CronExpression);

            EditJobView model = new EditJobView(JobName, Group, TriggerName, TriggerGroup, croneDate);

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditJob(EditJobView model)
        {

            JobKey key = new JobKey(model.JobName, model.Group);
            IJobDetail details = _scheduler.GetJobDetail(key).Result;
            var Type = details.JobType;


            await _scheduler.DeleteJob(key);

            string CroneText = MakeCroneDate(model.Crone);


            IJobDetail job5 = JobBuilder.Create<TestJob2Minutes>().WithIdentity(model.JobName, model.Group).StoreDurably().RequestRecovery().Build();

            await _scheduler.AddJob(job5, true);

            ITrigger trigger5 = TriggerBuilder.Create()
                                            .ForJob(job5)
                                            .WithIdentity(model.TriggerName, model.TriggerGroup)
                                            .StartNow()
                                           .WithCronSchedule(CroneText)
                                            .Build();

            await _scheduler.ScheduleJob(trigger5);

            return RedirectToAction("SchedulerDetails");
        }





    }
}