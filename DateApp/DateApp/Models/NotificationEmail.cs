using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    namespace DateApp.Models
    {
        public interface IGetPath
        {
            List<string> GetPathOfImage(List<string> Name);
        }

        public class GetPaht : IGetPath
        {
            IHostingEnvironment _env;

            public GetPaht(IHostingEnvironment _env)
            {
                this._env = _env;
            }

            public List<string> GetPathOfImage(List<string> Names)
            {
                List<string> Pathes = new List<string>();

                if (Names != null)
                {
                    foreach (var name in Names)
                    {

                        var pathToImage = _env.WebRootPath
                                   + Path.DirectorySeparatorChar.ToString()
                                   + "Images"
                                   + Path.DirectorySeparatorChar.ToString()
                                   + name;

                        Pathes.Add(pathToImage);


                    }
                }

                return Pathes;

            }
        }
               
        public interface IMakeDate
        {
            DateTime Time { get; set; }
            string Make();
        }

        public class MakeDate : IMakeDate
        {
            public DateTime Time { get; set; }

            public MakeDate(DateTime time)
            {
                Time = time;
            }



            public string CheckNumber(int number)
            {
                string Number;
                if (number < 10)
                {
                    Number = "0" + number.ToString();
                }
                else
                {
                    Number = number.ToString();
                }
                return Number;
            }

            public string Make()
            {
                string Day = CheckNumber(Time.Day);
                string Month = CheckNumber(Time.Month);
                string Year = Time.Year.ToString();
                string Hour = CheckNumber(Time.Hour);
                string Minute = CheckNumber(Time.Minute);

                return Day + "-" + Month + "-" + Year + " " + Hour + ":" + Minute;
            }
        }
               
        public interface ISendEmail
        {
            //List<LinkedResource> Resources { get; set; }
            //string PageHtml { get; set; }
            //SmtpClient Client { get; set; }

            string UserEmail { get; set; }
            bool Send(List<LinkedResource> Resources, string PageHtml, SmtpClient Client);
        }

        public class SendEmailPair : ISendEmail
        {
           
            public string UserEmail { get; set; }


            public SendEmailPair(string UserEmail)
            {

                this.UserEmail = UserEmail;

            }



            public bool Send(List<LinkedResource> Resources, string PageHtml, SmtpClient Client)
            {
                try
                {

                    using (var client = Client)
                    {

                        LinkedResource inlineLogo = Resources[0];
                        LinkedResource inlineLogo2 = Resources[1];
                        string body = PageHtml;

                        MailMessage newMail = new MailMessage
                        {
                            From = new MailAddress("DateApp@gmail.com"),
                            Subject = "Wydarzenia w DateApp " + DateTime.Now.ToShortDateString(),
                            Body = body,
                            IsBodyHtml = true,
                        };

                        var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                        view.LinkedResources.Add(inlineLogo);
                        view.LinkedResources.Add(inlineLogo2);
                        newMail.AlternateViews.Add(view);
                        newMail.To.Add(UserEmail);
                        client.Send(newMail);

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }


        }

        public class SendEmailMessage : ISendEmail
        {

            public string UserEmail { get; set; }


            public SendEmailMessage(string UserEmail)
            {

                this.UserEmail = UserEmail;

            }



            public bool Send(List<LinkedResource> Resources, string PageHtml, SmtpClient Client)
            {
                try
                {

                    using (var client = Client)
                    {

                        LinkedResource inlineLogo = Resources[0];
                        LinkedResource inlineLogo2 = Resources[1];
                        string body = PageHtml;

                        MailMessage newMail = new MailMessage
                        {
                            From = new MailAddress("DateApp@gmail.com"),
                            Subject = "Wydarzenia w DateApp " + DateTime.Now.ToShortDateString(),
                            Body = body,
                            IsBodyHtml = true,
                        };

                        var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                        view.LinkedResources.Add(inlineLogo);
                        view.LinkedResources.Add(inlineLogo2);
                        newMail.AlternateViews.Add(view);
                        newMail.To.Add(UserEmail);
                        client.Send(newMail);

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }


        }

        public class SendEmailLike : ISendEmail
        {

            public string UserEmail { get; set; }


            public SendEmailLike(string UserEmail)
            {

                this.UserEmail = UserEmail;

            }



            public bool Send(List<LinkedResource> Resources, string PageHtml, SmtpClient Client)
            {
                try
                {

                    using (var client = Client)
                    {

                        LinkedResource inlineLogo = Resources[0];                        
                        string body = PageHtml;

                        MailMessage newMail = new MailMessage
                        {
                            From = new MailAddress("DateApp@gmail.com"),
                            Subject = "Wydarzenia w DateApp " + DateTime.Now.ToShortDateString(),
                            Body = body,
                            IsBodyHtml = true,
                        };

                        var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                        view.LinkedResources.Add(inlineLogo);                       
                        newMail.AlternateViews.Add(view);
                        newMail.To.Add(UserEmail);
                        client.Send(newMail);

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }


        }

        public interface IMakePage
        {
            //string Date { get; set; }
            //List<LinkedResource> Resources { get; set; }
            string Email { get; set; }
            int Count { get; set; }
            string MakePage(string Date, List<LinkedResource> Resources);

        }

        public class MakePairPage : IMakePage
        {
            //public string Date { get; set; }
            //public List<LinkedResource> Resources { get; set; }
            public string Email { get; set; }
            public int Count { get; set; }


            public MakePairPage()
            {

            }


            public MakePairPage(string Email, int Count)
            {
                //this.Date = Date;
                //this.Resources = Resources;
                this.Email = Email;
                this.Count = Count;

            }



            public string MakePage(string Date, List<LinkedResource> Resources)
            {


                string body = string.Format(@"


    <div style = ""width:401px; height:880px; border:solid;"" >
 
         <div id = ""Main"" style = ""width:400px; height:600px;  background-color: #ffcccc;      "" >
           
           
    
                <div style = ""width:400px; height:60px;   "" >
     

                     <div class=""box"" style =""width:80px; float:left; height: 20px; "" >

                </div>
                <div class=""box"" style =""width:280px; float:left; height: 20px;"" >
                    <h3>Nowe Wydarzenia w DateApp</h3>
                </div>
                <div class=""box"" style =""width:40px; float:left; height: 20px;  "" >

                </div>
               
            </div>


            <div style = ""width:400px; height:80px;"" id =""Pair"" >
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4>Masz Nową Parę : " + Email + @"</h4>
    
                </div>
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4>" + Date + @"</h4>
                </div>

            </div>
            <div style = ""width:400px; height:255px;"" id =""PairPicture"">
                <div style = ""width:20px float:left; height: 20px;"" class=""box"" ></div>
                <img src=""cid:{0}"" alt =""Simply Easy Learning"" width =""150""
                     height =""250"" >
            </div>
            <div style = ""width:400px; height:80px;"" id =""Details"" >
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4> Łącznie nowych Par  " + Count + @" </h4>
                </div>
               

            </div>

            <div id = ""Main2"" >
                <img src=""cid:{1}"" alt=""Simply Easy Learning"" width =""400""
                     height =""400"" >
            </div>
        </div>
    </div> 


", Resources[0].ContentId, Resources[1].ContentId);

                return body;

            }
        }

        public class MakeMessagePage : IMakePage
        {
            
            public string Email { get; set; }
            public int Count { get; set; }


            public MakeMessagePage()
            {

            }


            public MakeMessagePage(string Email, int Count)
            {
                
                this.Email = Email;
                this.Count = Count;

            }



            public string MakePage(string Date, List<LinkedResource> Resources)
            {


                string body = string.Format(@"


                    <div style = ""width:401px; height:880px; border:solid;"" >

                         <div id = ""Main"" style = ""width:400px; height:600px;  background-color: #43cde6;      "" >



                                <div style = ""width:400px; height:60px;   "" >


                                     <div class=""box"" style =""width:80px; float:left; height: 20px; "" >

                                </div>
                                <div class=""box"" style =""width:280px; float:left; height: 20px;"" >
                                    <h3>Nowe Wydarzenia w DateApp</h3>
                                </div>
                                <div class=""box"" style =""width:40px; float:left; height: 20px;  "" >

                                </div>

                            </div>


                            <div style = ""width:400px; height:80px;"" id =""Pair"" >
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                                    <h4>Masz Nową Wiadomość od  " + Email + @"</h4>

                                </div>
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                                    <h4>" + Date+ @"</h4>
                                </div>

                            </div>
                            <div style = ""width:400px; height:255px;"" id =""PairPicture"">
                                <div style = ""width:20px float:left; height: 20px;"" class=""box"" ></div>
                                <img src=""cid:{0}"" alt =""Simply Easy Learning"" width =""150""
                                     height =""250"" >
                            </div>
                            <div style = ""width:400px; height:80px;"" id =""Details"" >
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                                    <h4> Łącznie nowych Wiadomości  " + Count + @" </h4>
                                </div>


                            </div>

                            <div id = ""Main2"" >
                                <img src=""cid:{1}"" alt=""Simply Easy Learning"" width =""400""
                                     height =""400"" >
                            </div>
                        </div>
                    </div> 


                ", Resources[0].ContentId, Resources[1].ContentId);

                return body;

            }
        }

        public class MakeLikePage : IMakePage
        {

            public string Email { get; set; }
            public int Count { get; set; }


            public MakeLikePage()
            {

            }


            public MakeLikePage(string Email/*, int Count*/)
            {

                this.Email = Email;
                //this.Count = Count;

            }



            public string MakePage(string Date, List<LinkedResource> Resources)
            {

                string body = string.Format(@"


                    <div style = ""width:401px; height:880px; border:solid;"" >

                         <div id = ""Main"" style = ""width:400px; height:600px;  background-color: #3366ff;      "" >



                                <div style = ""width:400px; height:60px;   "" >


                                     <div class=""box"" style =""width:80px; float:left; height: 20px; "" >

                                </div>
                                <div class=""box"" style =""width:280px; float:left; height: 20px;"" >
                                    <h3>Nowe Wydarzenia w DateApp</h3>
                                </div>
                                <div class=""box"" style =""width:40px; float:left; height: 20px;  "" >

                                </div>

                            </div>


                            <div style = ""width:400px; height:80px;"" id =""Pair"" >
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                                    <h4>Możesz wybierać masz 2 nowe polubienia  </h4>

                                </div>
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                                    <h4>" + Date + @"</h4>
                                </div>

                            </div>
                            <div style = ""width:400px; height:255px;"" id =""PairPicture"">
                                <div style = ""width:20px float:left; height: 20px;"" class=""box"" ></div>

                            </div>
                            <div style = ""width:400px; height:80px;"" id =""Details"" >
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >

                                </div>


                            </div>

                            <div id = ""Main2"" >
                                <img src=""cid:{0}"" alt=""Simply Easy Learning"" width =""400""
                                     height =""400"" >
                            </div>
                        </div>
                    </div> 


                ", Resources[0].ContentId);


                return body;


            }
        }

        public class MakeSuperLikePage : IMakePage
        {

            public string Email { get; set; }
            public int Count { get; set; }


            public MakeSuperLikePage()
            {

            }


            public MakeSuperLikePage(string Email/*, int Count*/)
            {

                this.Email = Email;
                //this.Count = Count;

            }



            public string MakePage(string Date, List<LinkedResource> Resources)
            {

                string body = string.Format(@"


                    <div style = ""width:401px; height:880px; border:solid;"" >

                         <div id = ""Main"" style = ""width:400px; height:600px;  background-color: #00e6e6;      "" >



                                <div style = ""width:400px; height:60px;   "" >


                                     <div class=""box"" style =""width:80px; float:left; height: 20px; "" >

                                </div>
                                <div class=""box"" style =""width:280px; float:left; height: 20px;"" >
                                    <h3>Nowe Wydarzenia w DateApp</h3>
                                </div>
                                <div class=""box"" style =""width:40px; float:left; height: 20px;  "" >

                                </div>

                            </div>


                            <div style = ""width:400px; height:80px;"" id =""Pair"" >
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                                    <h4>Możesz wybierać masz 2 nowe super polubienia  </h4>

                                </div>
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                                    <h4>" + Date + @"</h4>
                                </div>

                            </div>
                            <div style = ""width:400px; height:255px;"" id =""PairPicture"">
                                <div style = ""width:20px float:left; height: 20px;"" class=""box"" ></div>

                            </div>
                            <div style = ""width:400px; height:80px;"" id =""Details"" >
                                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                                </div>
                                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >

                                </div>


                            </div>

                            <div id = ""Main2"" >
                                <img src=""cid:{0}"" alt=""Simply Easy Learning"" width =""400""
                                     height =""400"" >
                            </div>
                        </div>
                    </div> 


                ", Resources[0].ContentId);


                return body;


            }
        }

        public interface ISetSmtpClient
        {

            SmtpClient SetClient();

        }

        public class SetSmtpClient : ISetSmtpClient
        {
            public SmtpClient SetClient()
            {
                var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
                var config = builder.Build();

                SmtpClient smtpClient = new SmtpClient(config["Data:Smtp:Host"])
                {
                    Port = int.Parse(config["Data:Smtp:Port"]),
                    Credentials = new NetworkCredential(config["Data:Smtp:Username"], config["Data:Smtp:Password"]),
                    EnableSsl = true,
                };

                return smtpClient;
            }
        }
        
        public interface ISetLinkedResource
        {

            List<LinkedResource> Set(List<string> Paths);

        }

        public class SetLinkedResource : ISetLinkedResource
        {
            public List<LinkedResource> Set(List<string> Paths)
            {
                List<LinkedResource> Resources = new List<LinkedResource>();

                if (Paths != null)
                {
                    foreach (var path in Paths)
                    {
                        LinkedResource inlineLogo = new LinkedResource(path, "image/jpg");

                        inlineLogo.ContentId = Guid.NewGuid().ToString();

                        Resources.Add(inlineLogo);

                    }


                }

                return Resources;


            }
        }
               
        /////////

        public abstract class NotificationEmail
        {
            public IHostingEnvironment _env;
            public IMakeDate makeDate;
            public IGetPath getPath;
            public ISendEmail sendEmail;
            public IMakePage makePage;
            public ISetSmtpClient setSmtpClient;
            public ISetLinkedResource setResource;
            public SmtpClient client;

            public DateTime Time { get; set; }
            public string HtmlPage;
            public string Date;
            public List<string> PhotoNames { get; set; }
            public List<string> Pathes;
            public List<LinkedResource> ResourcesLink;



            public void MakeEmail()
            {
                GetDate();

                SetSmtpClient();

                GetPathes(PhotoNames);

                MakeLiknedResource();

                MakePage();

            }


            public NotificationEmail()
            {

            }

            public void SetSmtpClient()
            {
                client = setSmtpClient.SetClient();
            }

            public void GetDate()
            {
                Date = makeDate.Make();
            }


            public void GetPathes(List<string> Names)
            {
                Pathes = getPath.GetPathOfImage(Names);
            }


            public bool SendEmail()
            {
                return sendEmail.Send(ResourcesLink, HtmlPage, client);
            }

            public void MakeLiknedResource()
            {
                ResourcesLink = setResource.Set(Pathes);
            }


            public void MakePage()
            {
                HtmlPage = makePage.MakePage(Date, ResourcesLink);
            }

        }

        public class PairNotificationEmail : NotificationEmail
        {

            string Email { get; set; }
            string UserEmail { get; set; }
            int Count { get; set; }


            public PairNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time, int PairCount, List<string> names)
            {
                _env = env;
                this.Email = Email;
                this.Time = Time;
                Count = PairCount;
                PhotoNames = names;
                this.UserEmail = UserEmail;


                makeDate = new MakeDate(Time);

                setSmtpClient = new SetSmtpClient();

                getPath = new GetPaht(_env);

                setResource = new SetLinkedResource();

                makePage = new MakePairPage(Email, Count);

                sendEmail = new SendEmailPair(UserEmail);

            }

        }        

        public class MessageNotificationEmail : NotificationEmail
        {

            string Email { get; set; }
            string UserEmail { get; set; }
            int Count { get; set; }


            public MessageNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time, int PairCount, List<string> names)
            {
                _env = env;
                this.Email = Email;
                this.Time = Time;
                Count = PairCount;
                PhotoNames = names;
                this.UserEmail = UserEmail;


                makeDate = new MakeDate(Time);

                setSmtpClient = new SetSmtpClient();

                getPath = new GetPaht(_env);

                setResource = new SetLinkedResource();

                makePage = new MakeMessagePage(Email, Count);

                sendEmail = new SendEmailMessage(UserEmail);

            }

        }

        public class LikeNotificationEmail : NotificationEmail
        {

            string Email { get; set; }
            string UserEmail { get; set; }
            int Count { get; set; }


            public LikeNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time,/* int PairCount,*/ List<string> names)
            {
                _env = env;
                this.Email = Email;
                this.Time = Time;
                //Count = PairCount;
                PhotoNames = names;
                this.UserEmail = UserEmail;


                makeDate = new MakeDate(Time);

                setSmtpClient = new SetSmtpClient();

                getPath = new GetPaht(_env);

                setResource = new SetLinkedResource();

                makePage = new MakeLikePage(Email/*, Count*/);

                sendEmail = new SendEmailLike(UserEmail);

            }

        }

        public class SuperLikeNotificationEmail : NotificationEmail
        {

            string Email { get; set; }
            string UserEmail { get; set; }
            int Count { get; set; }


            public SuperLikeNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time/*, int PairCount*/, List<string> names)
            {
                _env = env;
                this.Email = Email;
                this.Time = Time;
                //Count = PairCount;
                PhotoNames = names;
                this.UserEmail = UserEmail;


                makeDate = new MakeDate(Time);

                setSmtpClient = new SetSmtpClient();

                getPath = new GetPaht(_env);

                setResource = new SetLinkedResource();

                makePage = new MakeSuperLikePage(Email/*, Count*/);

                sendEmail = new SendEmailLike(UserEmail);

            }

        }                                 
              


    }

}
