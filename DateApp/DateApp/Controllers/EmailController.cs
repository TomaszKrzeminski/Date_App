using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace DateApp.Controllers
{
    public class EmailController : Controller
    {
        private IHostingEnvironment _env;

        public EmailController(IHostingEnvironment env)
        {
            _env = env;
        }




        public IActionResult Send()
        {

            using (var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tomaszkrzeminski21081985@gmail.com", "Martyna1985@"),
                EnableSsl = true,
            })
            {




                var pathToImage = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Images"
                            + Path.DirectorySeparatorChar.ToString()
                            + "PairImage.jpg";


                var inlineLogo = new LinkedResource(pathToImage, "image/jpg");
                inlineLogo.ContentId = Guid.NewGuid().ToString();




                var pathToImage2 = _env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Images"
                           + Path.DirectorySeparatorChar.ToString()
                           + "test.jpg";


                var inlineLogo2 = new LinkedResource(pathToImage2, "image/jpg");
                inlineLogo2.ContentId = Guid.NewGuid().ToString();










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
                    <h4>Masz Nową Parę  Email</h4>
    
                </div>
                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

                </div>
                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
                    <h4>  19-9-2020  10:06</h4>
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
                    <h4> Łącznie nowych Par Number </h4>
                </div>
               

            </div>

            <div id = ""Main2"" >
                <img src=""cid:{0}"" alt=""Simply Easy Learning"" width =""400""
                     height =""400"" >
            </div>
        </div>
    </div> 


", inlineLogo.ContentId,inlineLogo2.ContentId);






                MailMessage newMail = new MailMessage
                {
                    From = new MailAddress("DateApp@gmail.com"),
                    Subject = "Notifications",
                    Body = body,
                    IsBodyHtml = true,
                };



                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                view.LinkedResources.Add(inlineLogo);
                view.LinkedResources.Add(inlineLogo2);
                newMail.AlternateViews.Add(view);
                newMail.To.Add("zdalnerepo1985@gmail.com");
                client.Send(newMail);


                return View();

            }


        }




//        public IActionResult Send()
//        {

//            using (var client = new SmtpClient("smtp.gmail.com")
//            {
//                Port = 587,
//                Credentials = new NetworkCredential("tomaszkrzeminski21081985@gmail.com", "Martyna1985@"),
//                EnableSsl = true,
//            })
//            {




//                var pathToImage = _env.WebRootPath
//                            + Path.DirectorySeparatorChar.ToString()
//                            + "Images"
//                            + Path.DirectorySeparatorChar.ToString()
//                            + "PairImage.jpg";


//                var inlineLogo = new LinkedResource(pathToImage, "image/jpg");
//                inlineLogo.ContentId = Guid.NewGuid().ToString();






//                string body = string.Format(@"





//    <div style = ""width:401px; height:880px; border:solid;"" >
 
//         <div id = ""Main"" style = ""width:400px; height:600px;  background-color: #ffcccc;      "" >
           
           
    
//                <div style = ""width:400px; height:60px;   "" >
     

//                     <div class=""box"" style =""width:80px; float:left; height: 20px; "" >

//                </div>
//                <div class=""box"" style =""width:280px; float:left; height: 20px;"" >
//                    <h3>Nowe Wydarzenia w DateApp</h3>
//                </div>
//                <div class=""box"" style =""width:40px; float:left; height: 20px;  "" >

//                </div>
               
//            </div>


//            <div style = ""width:400px; height:80px;"" id =""Pair"" >
//                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

//                </div>
//                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
//                    <h4>Masz Nową Parę  Email</h4>
    
//                </div>
//                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

//                </div>
//                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
//                    <h4>  19-9-2020  10:06</h4>
//                </div>

//            </div>
//            <div style = ""width:400px; height:255px;"" id =""PairPicture"">
//                <div style = ""width:20px float:left; height: 20px;"" class=""box"" ></div>
//                <img src = "" / images/test.jpg"" alt =""Simply Easy Learning"" width =""150""
//                     height =""250"" >
//            </div>
//            <div style = ""width:400px; height:80px;"" id =""Details"" >
//                <div style = ""width:20px; float:left; height: 20px;"" class=""box"" >

//                </div>
//                <div style = ""width:380px; float:left; height: 20px;"" class=""box"" >
//                    <h4> Łącznie nowych Par Number </h4>
//                </div>
               

//            </div>

//            <div id = ""Main2"" >
//                <img src=""cid:{0}"" alt=""Simply Easy Learning"" width =""400""
//                     height =""400"" >
//            </div>
//        </div>
//    </div> 


//", inlineLogo.ContentId);






//                MailMessage newMail = new MailMessage
//                {
//                    From = new MailAddress("DateApp@gmail.com"),
//                    Subject = "Notifications",
//                    Body = body,
//                    IsBodyHtml = true,
//                };



//                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
//                view.LinkedResources.Add(inlineLogo);
//                newMail.AlternateViews.Add(view);
//                newMail.To.Add("zdalnerepo1985@gmail.com");
//                client.Send(newMail);


//                return View();

//            }


//        }

























    }
}