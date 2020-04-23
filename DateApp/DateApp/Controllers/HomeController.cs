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
    public class HomeController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;

        public PictureType GetPictureType(string PictureNumber)
        {
            int number = Convert.ToInt32(PictureNumber);
            PictureType type = (PictureType)number;
            return type;
        }


        public HomeController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
        }

        public IActionResult StartPage()
        {
            return View();
        }

        public IActionResult PictureAdder()
        {
            string Id = userManager.GetUserId(HttpContext.User);

            SearchDetails details = repository.GetUserDetails(Id);
            UserDetailsModel model = new UserDetailsModel() { DetailsId = details.SearchDetailsId, MainPhotoPath = details.MainPhotoPath ?? "/AppPictures/photo.png", PhotoPath1 = details.PhotoPath1 ?? "/AppPictures/photo.png", PhotoPath2 = details.PhotoPath2 ?? "/AppPictures/photo.png", PhotoPath3 = details.PhotoPath3 ?? "/AppPictures/photo.png", Description = details.Description, CityOfResidence = details.CityOfResidence, JobPosition = details.JobPosition, CompanyName = details.CompanyName, School = details.School, UserId = Id };

            return View(model);
        }

        [HttpPost]
        public IActionResult PictureAdder(UserDetailsModel model)
        {
            bool success = repository.ChangeUserDetails(model);
            if (success)
            {
                return RedirectToRoute(new { controller = "Home", action = "PictureAdder", Id = "MyId" });
            }
            else
            {
                string Message = "Dodanie zmian się nie powiodło";
                return View("Error", Message);
            }

        }


        [HttpPost]
        public IActionResult RemovePicture(string Number)
        {

            bool success = false;

            if (Number != null)
            {
                string Id = userManager.GetUserId(HttpContext.User);
                PictureType type = GetPictureType(Number);
                success = repository.RemovePicture(Id, type);

            }




            if (success)
            {
                return RedirectToRoute(new { controller = "Home", action = "PictureAdder", Id = "MyId" });
            }
            else
            {
                string Message = "Usunięcie zdjęcia się nie powiodło";
                return View("Error", Message);
            }


        }


        //Dodać sprawdzenie czy plik jpg

        [HttpPost]
        public async Task<IActionResult> AddPictureAsync(IFormFile file, string PictureNumber)
        {
            string Message = "Dodanie zdjęcia nie powiodło się !!!";


            bool success = false;

            if (file != null)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "Images");
                string FilePath;
                if (file.Length > 0)
                {

                    if (Path.GetExtension(file.FileName) == ".jpg")
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            FilePath = "/Images/" + file.FileName;
                            await file.CopyToAsync(fileStream);
                        }
                        string Id = userManager.GetUserId(HttpContext.User);
                        PictureType type = GetPictureType(PictureNumber);
                        success = repository.AddPicture(Id, type, FilePath);
                    }
                    else
                    {
                        Message = "Zdjęcie musi być w formacie jpg";
                        success = false;
                    }




                }


                if (success)
                {
                    return RedirectToRoute(new { controller = "Home", action = "PictureAdder", Id = "MyId" });
                }
                else
                {

                    return View("Error", Message);
                }



            }
            return RedirectToRoute(new { controller = "Home", action = "PictureAdder", Id = "MyId" });


        }




    }
}