using System;
using System.IO;
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


        

        [HttpPost]
        public IActionResult SetShowProfile(bool Show)
        {
            string Id = userManager.GetUserId(HttpContext.User);
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
            string Id = userManager.GetUserId(HttpContext.User);
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
            string Id = userManager.GetUserId(HttpContext.User);
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

            string Id = userManager.GetUserId(HttpContext.User);
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



        public IActionResult ChangePhoneNumber()
        {
            ChangePhoneNumberView model = new ChangePhoneNumberView();
            string Id = userManager.GetUserId(HttpContext.User);
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

            string Id = userManager.GetUserId(HttpContext.User);

            SearchDetails details = repository.GetUserDetails(Id);
            Coordinates coordinates = repository.GetCoordinates(Id);

            if(details==null)
            {
                return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
            }

            UserDetailsModel detailsmodel = new UserDetailsModel() { DetailsId = details.Id, MainPhotoPath = details.MainPhotoPath ?? "/AppPictures/photo.png", PhotoPath1 = details.PhotoPath1 ?? "/AppPictures/photo.png", PhotoPath2 = details.PhotoPath2 ?? "/AppPictures/photo.png", PhotoPath3 = details.PhotoPath3 ?? "/AppPictures/photo.png", Description = details.Description, CityOfResidence = details.CityOfResidence, JobPosition = details.JobPosition, CompanyName = details.CompanyName, School = details.School, UserId = Id };

            UserSettingsModel settingsmodel = new UserSettingsModel() { MainPhotoPath = details.MainPhotoPath, Name = details.User.UserName, Surname = details.User.Surname, Likes = details.Likes, SuperLikes = details.SuperLikes, Email = details.User.Email, PhoneNumber = details.User.PhoneNumber ?? "Update", Localization = details.CityOfResidence, SearchAge = details.SearchAge, Distance = details.SearchDistance, SearchSex = details.SearchSex ?? "Male", ShowProfile = details.ShowProfile};
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
                string Id = userManager.GetUserId(HttpContext.User);
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