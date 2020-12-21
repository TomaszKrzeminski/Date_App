using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Hubs;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DateApp.Controllers
{
    public class VideoController : Controller
    {


        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private Func<Task<AppUser>> GetUser;
        private IHubContext<VideoConnectionHub> connectionContext;





        public VideoController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env, IHubContext<VideoConnectionHub> connectionContext, Func<Task<AppUser>> GetUser = null)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
            this.connectionContext = connectionContext;

            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }





        }

        public IActionResult Test()
        {
            return View();
        }
        public IActionResult ScreenShot()
        {
            return View();
        }
        public JsonResult Add(string imageData)
        {
            bool check = false;
            try
            {              

                    string UserId = GetUser().Result.Id;

                var uploads = Path.Combine(_environment.WebRootPath, "ScreenShots");
                string FilePath;
                if (imageData != null && imageData.Length > 0)
                {

                    Guid obj = Guid.NewGuid();
                    string guid = obj.ToString();
                    string FileName = "ScreenShot"+guid+UserId + ".jpg";
                    string PathText = Path.Combine(uploads, FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, FileName), FileMode.Create))
                    {
                        FilePath = "/ScreenShots/" + FileName;
                        //await file.CopyToAsync(fileStream);

                        using (BinaryWriter bw = new BinaryWriter(fileStream))
                        {
                            byte[] data = Convert.FromBase64String(imageData);
                            bw.Write(data);
                            bw.Close();
                        }


                    }

                    ////////
                    check = repository.SetScreenShotAsMainPhoto(FilePath, UserId);

                    if (check)
                    {
                        return new JsonResult("Success");
                    }
                    else
                    {
                        return new JsonResult("Error");
                    }

                }
                else
                {
                    return new JsonResult("Error");
                }

            }
            catch (Exception exception)
            {
                return new JsonResult("Error");
            }


        }
        public IActionResult VideoCall(string ReceiverId)
        {
            string CallerId = GetUser().Result.Id;
            VideoCallViewModel model = new VideoCallViewModel(ReceiverId, CallerId);
            return View(model);
        }
       

    }
}