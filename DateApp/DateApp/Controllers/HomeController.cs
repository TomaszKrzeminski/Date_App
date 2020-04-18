using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;

        public HomeController(IRepository repo,UserManager<AppUser> userMgr)
        {
            repository = repo;
            userManager = userMgr;
        }

        public IActionResult StartPage()
        {
            return View();
        }

        public IActionResult PictureAdder()
        {
            string Id = userManager.GetUserId(HttpContext.User);
        
            SearchDetails details = repository.GetUserDetails(Id);
            UserDetailsModel model = new UserDetailsModel() {DetailsId=details.SearchDetailsId ,MainPhotoPath=details.MainPhotoPath??"/AppPictures/photo.png",PhotoPath1=details.PhotoPath1 ?? "/AppPictures/photo.png", PhotoPath2 = details.PhotoPath2 ?? "/AppPictures/photo.png", PhotoPath3 = details.PhotoPath3 ?? "/AppPictures/photo.png", Description=details.Description,CityOfResidence=details.CityOfResidence,JobPosition=details.JobPosition,CompanyName=details.CompanyName,School=details.School ,UserId=Id};                                            
            
            return View(model);
        }

        [HttpPost]
        public IActionResult PictureAdder(UserDetailsModel model)
        {
        bool success=repository.ChangeUserDetails(model);
            if(success)
            {
return RedirectToRoute(new { controller = "Home", action = "PictureAdder", Id = "MyId" });
            }
            else
            {
                string Message = "Dodanie zmian się nie powiodło";
                return View("Error", Message);
            }
            
        }

    }
}