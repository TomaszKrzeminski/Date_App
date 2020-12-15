using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class VideoController : Controller
    {


        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private Func<Task<AppUser>> GetUser;






        public VideoController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env, Func<Task<AppUser>> GetUser = null)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;


            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }





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

        //[HttpPost]
        //public JsonResult Add(string imageData)
        //{
        //    bool check = false;
        //    try
        //    {
        //        string UserId = GetUser().Result.Id;
        //        var folderName = @"ScreenShots/";
        //        var webRootPath = _environment.WebRootPath;
        //        var newPath = Path.Combine(webRootPath, folderName);
        //        string fileNameWitPath = newPath +UserId+ DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".jpg";
        //        using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
        //        {
        //            using (BinaryWriter bw = new BinaryWriter(fs))
        //            {
        //                byte[] data = Convert.FromBase64String(imageData);
        //                bw.Write(data);
        //                bw.Close();
        //            }
        //        }

        //        check = repository.SetScreenShotAsMainPhoto(fileNameWitPath, UserId);

        //        if(check)
        //        {
        //            return new JsonResult("Success");
        //        }
        //        else
        //        {
        //            return new JsonResult("Error");
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        return new JsonResult("Error");
        //    }


        //}    

    }
}