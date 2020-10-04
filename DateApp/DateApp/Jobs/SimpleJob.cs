
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Jobs
{

    public interface IEmailService
    {
        void Send(string receiver, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        public void Send(string receiver, string subject, string body)
        {
            Debug.WriteLine($"Sending email to {receiver} with subject {subject} and body {body}");
        }
    }






    public class SimpleJob : IJob
    {
        IEmailService _emailService;
        public SimpleJob(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _emailService.Send("info@devhow.net", "Quartz.net DI", "Dependency injection in quartz");
        }
    }
}
