using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DateApp.Models
{

    public interface INotificationEmail
    {
        string GetPathOfImage(string Name);
        string CheckNumber(int number);
        string MakePage(LinkedResource photoPair, LinkedResource pagePhoto, string pairEmail, DateTime pairTime, int PairCount);
        bool SendEmail();

    }

    public class PairNotificationEmail : INotificationEmail
    {
        public string UserEmail { get; set; }
        public string PairEmail { get; set; }
        public DateTime PairTime { get; set; }
        public int Count { get; set; }
        public string PairPhoto { get; set; }
        public string PagePhoto { get; set; }

        IHostingEnvironment _env;

        public PairNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time, int PairCount, string PairPhoto, string PagePhoto)
        {
            _env = env;
            PairEmail = Email;
            PairTime = Time;
            Count = PairCount;
            this.UserEmail = UserEmail;
            this.PairPhoto = PairPhoto;
            this.PagePhoto = PagePhoto;
        }


        public string GetPathOfImage(string Name)
        {
            var pathToImage = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Images"
                            + Path.DirectorySeparatorChar.ToString()
                            + Name;
            return pathToImage;
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




        public string MakePage(LinkedResource photoPair, LinkedResource pagePhoto, string pairEmail, DateTime pairTime, int PairCount)
        {


            string Day = CheckNumber(pairTime.Day);
            string Month = CheckNumber(pairTime.Month);
            string Year = pairTime.Year.ToString();
            string Hour = CheckNumber(pairTime.Hour);
            string Minute = CheckNumber(pairTime.Minute);



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
                    <h4>Masz Nową Parę : " + pairEmail + @"</h4>
    
                </div>
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4>" + Day + "-" + Month + "-" + Year + " " + Hour + ":" + Minute + @"</h4>
                </div>

            </div>
            <div style = ""width:400px; height:255px;"" id =""PairPicture"">
                <div style = ""width:20px float:left; height: 20px;"" class=""box"" ></div>
                <img src=""cid:{1}"" alt =""Simply Easy Learning"" width =""150""
                     height =""250"" >
            </div>
            <div style = ""width:400px; height:80px;"" id =""Details"" >
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4> Łącznie nowych Par  " + PairCount + @" </h4>
                </div>
               

            </div>

            <div id = ""Main2"" >
                <img src=""cid:{0}"" alt=""Simply Easy Learning"" width =""400""
                     height =""400"" >
            </div>
        </div>
    </div> 


", photoPair.ContentId, pagePhoto.ContentId);


            return body;


        }






        public bool SendEmail()
        {

            try
            {

                using (var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("tomaszkrzeminski21081985@gmail.com", "Martyna1985@"),
                    EnableSsl = true,
                })
                {

                    var pathToImage = GetPathOfImage(PagePhoto);

                    var pathToImage2 = GetPathOfImage(PairPhoto);

                    var inlineLogo = new LinkedResource(pathToImage, "image/jpg");

                    inlineLogo.ContentId = Guid.NewGuid().ToString();

                    var inlineLogo2 = new LinkedResource(pathToImage2, "image/jpg");

                    inlineLogo2.ContentId = Guid.NewGuid().ToString();

                    string body = MakePage(inlineLogo, inlineLogo2, PairEmail, PairTime, Count);







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

    public class MessageNotificationEmail : INotificationEmail
    {

        public string UserEmail { get; set; }
        public string PairEmail { get; set; }
        public DateTime PairTime { get; set; }
        public int Count { get; set; }
        public string PairPhoto { get; set; }
        public string PagePhoto { get; set; }

        IHostingEnvironment _env;

        public MessageNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time, int MessageCount, string PairPhoto, string PagePhoto)
        {
            _env = env;
            PairEmail = Email;
            PairTime = Time;
            Count = MessageCount;
            this.UserEmail = UserEmail;
            this.PairPhoto = PairPhoto;
            this.PagePhoto = PagePhoto;
        }

        //Same
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


        //Same
        public string GetPathOfImage(string Name)
        {
            var pathToImage = _env.WebRootPath
                             + Path.DirectorySeparatorChar.ToString()
                             + "Images"
                             + Path.DirectorySeparatorChar.ToString()
                             + Name;
            return pathToImage;
        }



        /// Other

        public string MakePage(LinkedResource photoPair, LinkedResource pagePhoto, string pairEmail, DateTime pairTime, int MessageCount)
        {

            string Day = CheckNumber(pairTime.Day);
            string Month = CheckNumber(pairTime.Month);
            string Year = pairTime.Year.ToString();
            string Hour = CheckNumber(pairTime.Hour);
            string Minute = CheckNumber(pairTime.Minute);



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
                    <h4>Masz Nową Wiadomość od  " + pairEmail + @"</h4>
    
                </div>
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4>" + Day + "-" + Month + "-" + Year + " " + Hour + ":" + Minute + @"</h4>
                </div>

            </div>
            <div style = ""width:400px; height:255px;"" id =""PairPicture"">
                <div style = ""width:20px float:left; height: 20px;"" class=""box"" ></div>
                <img src=""cid:{1}"" alt =""Simply Easy Learning"" width =""150""
                     height =""250"" >
            </div>
            <div style = ""width:400px; height:80px;"" id =""Details"" >
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4> Łącznie nowych Wiadomości  " + MessageCount + @" </h4>
                </div>
               

            </div>

            <div id = ""Main2"" >
                <img src=""cid:{0}"" alt=""Simply Easy Learning"" width =""400""
                     height =""400"" >
            </div>
        </div>
    </div> 


", photoPair.ContentId, pagePhoto.ContentId);


            return body;





        }

        //Same
        public bool SendEmail()
        {



            try
            {

                using (var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("tomaszkrzeminski21081985@gmail.com", "Martyna1985@"),
                    EnableSsl = true,
                })
                {

                    var pathToImage = GetPathOfImage(PagePhoto);

                    var pathToImage2 = GetPathOfImage(PairPhoto);

                    var inlineLogo = new LinkedResource(pathToImage, "image/jpg");

                    inlineLogo.ContentId = Guid.NewGuid().ToString();

                    var inlineLogo2 = new LinkedResource(pathToImage2, "image/jpg");

                    inlineLogo2.ContentId = Guid.NewGuid().ToString();

                    string body = MakePage(inlineLogo, inlineLogo2, PairEmail, PairTime, Count);







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
    
    public class LikeNotificationEmail : INotificationEmail
    {


        public string UserEmail { get; set; }
        public string PairEmail { get; set; }
        public DateTime PairTime { get; set; }
        public int Count { get; set; }
        public string PairPhoto { get; set; }
        public string PagePhoto { get; set; }

        IHostingEnvironment _env;

        public LikeNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time, int MessageCount, string PairPhoto, string PagePhoto)
        {
            _env = env;
            PairEmail = Email;
            PairTime = Time;
            Count = MessageCount;
            this.UserEmail = UserEmail;
            this.PairPhoto = PairPhoto;
            this.PagePhoto = PagePhoto;
        }

        //Same
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


        //Same
        public string GetPathOfImage(string Name)
        {
            var pathToImage = _env.WebRootPath
                             + Path.DirectorySeparatorChar.ToString()
                             + "Images"
                             + Path.DirectorySeparatorChar.ToString()
                             + Name;
            return pathToImage;
        }



        /// Other

        public string MakePage(LinkedResource pagePhoto, LinkedResource pairPhoto, string pairEmail, DateTime pairTime, int MessageCount)
        {

            string Day = CheckNumber(pairTime.Day);
            string Month = CheckNumber(pairTime.Month);
            string Year = pairTime.Year.ToString();
            string Hour = CheckNumber(pairTime.Hour);
            string Minute = CheckNumber(pairTime.Minute);



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
                    <h4>" + Day + "-" + Month + "-" + Year + " " + Hour + ":" + Minute + @"</h4>
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


", pagePhoto.ContentId);


            return body;





        }

        //Same Pair Photo check if not null
        public bool SendEmail()
        {



            try
            {

                using (var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("tomaszkrzeminski21081985@gmail.com", "Martyna1985@"),
                    EnableSsl = true,
                })
                {

                    var pathToImage = GetPathOfImage(PagePhoto);

                    //tutaj tego nie dodaje

                    LinkedResource inlineLogo2;

                    if (PairPhoto != null)
                    {
                        var pathToImage2 = GetPathOfImage(PairPhoto);
                        inlineLogo2 = new LinkedResource(pathToImage2, "image/jpg");
                        inlineLogo2.ContentId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        inlineLogo2 = null;
                    }

                    var inlineLogo = new LinkedResource(pathToImage, "image/jpg");

                    inlineLogo.ContentId = Guid.NewGuid().ToString();





                    string body = MakePage(inlineLogo, inlineLogo2, PairEmail, PairTime, Count);







                    MailMessage newMail = new MailMessage
                    {
                        From = new MailAddress("DateApp@gmail.com"),
                        Subject = "Wydarzenia w DateApp " + DateTime.Now.ToShortDateString(),
                        Body = body,
                        IsBodyHtml = true,
                    };



                    var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    view.LinkedResources.Add(inlineLogo);

                    if (inlineLogo2 != null)
                    {
                        view.LinkedResources.Add(inlineLogo2);
                    }


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
       
    public class SuperLikeNotificationEmail : INotificationEmail
    {


        public string UserEmail { get; set; }
        public string PairEmail { get; set; }
        public DateTime PairTime { get; set; }
        public int Count { get; set; }
        public string PairPhoto { get; set; }
        public string PagePhoto { get; set; }

        IHostingEnvironment _env;

        public SuperLikeNotificationEmail(IHostingEnvironment env, string UserEmail, string Email, DateTime Time, int MessageCount, string PairPhoto, string PagePhoto)
        {
            _env = env;
            PairEmail = Email;
            PairTime = Time;
            Count = MessageCount;
            this.UserEmail = UserEmail;
            this.PairPhoto = PairPhoto;
            this.PagePhoto = PagePhoto;
        }

        //Same
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


        //Same
        public string GetPathOfImage(string Name)
        {
            var pathToImage = _env.WebRootPath
                             + Path.DirectorySeparatorChar.ToString()
                             + "Images"
                             + Path.DirectorySeparatorChar.ToString()
                             + Name;
            return pathToImage;
        }



        /// Other

        public string MakePage(LinkedResource pagePhoto, LinkedResource pairPhoto, string pairEmail, DateTime pairTime, int MessageCount)
        {

            string Day = CheckNumber(pairTime.Day);
            string Month = CheckNumber(pairTime.Month);
            string Year = pairTime.Year.ToString();
            string Hour = CheckNumber(pairTime.Hour);
            string Minute = CheckNumber(pairTime.Minute);



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
                    <h4>" + Day + "-" + Month + "-" + Year + " " + Hour + ":" + Minute + @"</h4>
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


", pagePhoto.ContentId);


            return body;





        }

        //Same Pair Photo check if not null
        public bool SendEmail()
        {



            try
            {

                using (var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("tomaszkrzeminski21081985@gmail.com", "Martyna1985@"),
                    EnableSsl = true,
                })
                {

                    var pathToImage = GetPathOfImage(PagePhoto);

                    //tutaj tego nie dodaje

                    LinkedResource inlineLogo2;

                    if (PairPhoto != null)
                    {
                        var pathToImage2 = GetPathOfImage(PairPhoto);
                        inlineLogo2 = new LinkedResource(pathToImage2, "image/jpg");
                        inlineLogo2.ContentId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        inlineLogo2 = null;
                    }

                    var inlineLogo = new LinkedResource(pathToImage, "image/jpg");

                    inlineLogo.ContentId = Guid.NewGuid().ToString();





                    string body = MakePage(inlineLogo, inlineLogo2, PairEmail, PairTime, Count);







                    MailMessage newMail = new MailMessage
                    {
                        From = new MailAddress("DateApp@gmail.com"),
                        Subject = "Wydarzenia w DateApp " + DateTime.Now.ToShortDateString(),
                        Body = body,
                        IsBodyHtml = true,
                    };



                    var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    view.LinkedResources.Add(inlineLogo);

                    if (inlineLogo2 != null)
                    {
                        view.LinkedResources.Add(inlineLogo2);
                    }


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
















}






