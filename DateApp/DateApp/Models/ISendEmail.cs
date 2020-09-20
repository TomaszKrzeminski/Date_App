using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace DateApp.Models
{
    interface ISendEmail
    {
        string SenderEmail { get; set; }

        string ReceiverEmail { get; set; }


        bool SendEmail(string SenderEmail, string ReceiverEmail, string EmailContent);




    }


    public class SendEmail : ISendEmail
    {
        public string SenderEmail { get ; set; }
        public string ReceiverEmail { get; set; }

        bool ISendEmail.SendEmail(string SenderEmail, string ReceiverEmail, string EmailContent)
        {

            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin",
            "koral2323@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("User",
            "tomaszkrzeminski21081985@gmail.com");
            message.To.Add(to);

            message.Subject = "Temat maila";


            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
            bodyBuilder.TextBody = "Hello World!";

            message.Body = bodyBuilder.ToMessageBody();


            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, true);
            client.Authenticate("koral2323@gmail.com", "Daria21081985@");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();







            return true;

        }
    }






}
