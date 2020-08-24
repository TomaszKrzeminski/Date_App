using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace DateApp.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private Func<Task<AppUser>> GetUser;
      





        public HomeController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env,Func<Task<AppUser>> GetUser = null)
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


        public PictureType GetPictureType(string PictureNumber)
        {
            PictureType type = new PictureType();
            int number;
            try
            {
                number = Convert.ToInt32(PictureNumber);
            }
            catch
            {
                number = 0;
            }


            if (number > 3 || number < 0)
            {
                number = 0;
            }
            else
            {
              type = (PictureType)number;
            }

            return type;
        }

        [HttpPost]
        public IActionResult SetShowProfile(bool Show)
        {
            //string Id = userManager.GetUserId(HttpContext.User);
            string Id = GetUser().Result.Id;
            bool succes = repository.SetShowProfile(Id, Show);
            if (succes)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }
            else
            {
                string Message = "Zmiana wieku nie powiodła się";
                return View("Error", Message);
            }

        }

        [HttpPost]
        public IActionResult SetAge(int Age)
        {
            string Id = GetUser().Result.Id;
            bool succes = repository.SetSearchAge(Id, Age);
            if (succes)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }
            else
            {
                string Message = "Zmiana wieku nie powiodła się";
                return View("Error", Message);
            }

        }

        [HttpPost]
        public IActionResult SetDistance(int Distance)
        {
            string Id = GetUser().Result.Id;
            bool succes = repository.SetDistance(Id, Distance);

            if (succes)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }
            else
            {
                string Message = "Zmiana Dystansu nie powiodła się";
                return View("Error", Message);
            }

        }


        [HttpPost]
        public IActionResult SetSearchSex(string SearchSex)
        {
            string Id = GetUser().Result.Id;
            bool succes = repository.ChangeSearchSex(SearchSex, Id);

            if (succes)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }
            else
            {
                string Message = "Zmiana poszukiwanej płci nie powiodła się";
                return View("Error", Message);
            }
        }



        public IActionResult StartPage()
        {
            return View();
        }

        public IActionResult ChangePhoneNumber()
        {
            ChangePhoneNumberView model = new ChangePhoneNumberView();
            string Id = GetUser().Result.Id;
            model.PhoneNumber = repository.GetPhoneNumber(Id);
            model.UserId = Id;
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangePhoneNumber(ChangePhoneNumberView model)
        {
            bool succes = false;
            succes = repository.ChangePhoneNumber(model.UserId, model.PhoneNumber);
            if (succes)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }
            else
            {
                string Message = "Zmiana numeru nie powiodła się";
                return View("Error", Message);
            }


        }


        public IActionResult TomTom()
        {
            return View();
        }




        public IActionResult Panel()
        {

            string Id = GetUser().Result.Id;

            SearchDetails details = repository.GetUserDetails(Id);
            Coordinates coordinates = repository.GetCoordinates(Id);

            if (details == null)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }

            UserDetailsModel detailsmodel = new UserDetailsModel() { DetailsId = details.Id, MainPhotoPath = details.MainPhotoPath ?? "/AppPictures/photo.png", PhotoPath1 = details.PhotoPath1 ?? "/AppPictures/photo.png", PhotoPath2 = details.PhotoPath2 ?? "/AppPictures/photo.png", PhotoPath3 = details.PhotoPath3 ?? "/AppPictures/photo.png", Description = details.Description, CityOfResidence = details.CityOfResidence, JobPosition = details.JobPosition, CompanyName = details.CompanyName, School = details.School, UserId = Id };

            UserSettingsModel settingsmodel = new UserSettingsModel() { MainPhotoPath = details.MainPhotoPath, Name = details.User.UserName, Surname = details.User.Surname, Likes = details.Likes, SuperLikes = details.SuperLikes, Email = details.User.Email, PhoneNumber = details.User.PhoneNumber ?? "Update", Localization = details.CityOfResidence, SearchAge = details.SearchAge, Distance = details.SearchDistance, SearchSex = details.SearchSex ?? "Male", ShowProfile = details.ShowProfile };
            settingsmodel.Coordinates.Latitude = coordinates.Latitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            settingsmodel.Coordinates.Longitude = coordinates.Longitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            settingsmodel.SetSex(details.User);

            PanelViewModel model = new PanelViewModel(detailsmodel, settingsmodel);

            return View(model);
        }


        [HttpPost]
        public IActionResult PictureAdder(UserDetailsModel model)
        {
            bool success = repository.ChangeUserDetails(model);
            if (success)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
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
                string Id = GetUser().Result.Id;
                PictureType type = GetPictureType(Number);
                success = repository.RemovePicture(Id, type);

            }


            if (success)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }
            else
            {
                string Message = "Usunięcie zdjęcia się nie powiodło";
                return View("Error", Message);
            }


        }


        //Dodać sprawdzenie czy plik jpg

        //[HttpPost]
        //public async Task<IActionResult> AddPictureAsync(IFormFile file, string PictureNumber)
        //{
        //    string Message = "Dodanie zdjęcia nie powiodło się !!!";


        //    bool success = false;

        //    if (file != null)
        //    {
        //        var uploads = Path.Combine(_environment.WebRootPath, "Images");
        //        string FilePath;
        //        if (file.Length > 0)
        //        {

        //            if (Path.GetExtension(file.FileName) == ".jpg")
        //            {
        //                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
        //                {
        //                    FilePath = "/Images/" + file.FileName;
        //                    await file.CopyToAsync(fileStream);
        //                }
        //                string Id = GetUser().Result.Id;
        //                PictureType type = GetPictureType(PictureNumber);
        //                success = repository.AddPicture(Id, type, FilePath);
        //            }
        //            else
        //            {
        //                Message = "Zdjęcie musi być w formacie jpg";
        //                success = false;
        //            }




        //        }


        //        if (success)
        //        {
        //            return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
        //        }
        //        else
        //        {

        //            return View("Error", Message);
        //        }



        //    }
        //    return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });


        //}



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

                        string PathText = Path.Combine(uploads, file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            FilePath = "/Images/" + file.FileName;
                            await file.CopyToAsync(fileStream);
                        }
                        string Id = GetUser().Result.Id;
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
                    return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
                }
                else
                {

                    return View("Error", Message);
                }



            }
            return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });


        }















    }
}