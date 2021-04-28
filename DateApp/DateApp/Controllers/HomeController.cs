using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Xml.Linq;
using DateApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace DateApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private Func<Task<AppUser>> GetUser;
        private IConfiguration configuration;

        public HomeController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env,IConfiguration configuration, Func<Task<AppUser>> GetUser = null)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
            this.configuration = configuration;



            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }

        }


        public string ChangeToMinutes(int seconds)
        {

            if (seconds > 60)
            {
                return (seconds / 60).ToString() + " minutes";
            }
            else
            {
                return "less than 1 minute";
            }
        }


        public string ChangeMetersToKM(int meters)
        {
            if (meters > 1000)
            {
                return (meters / 1000).ToString() + " km";
            }
            else
            {
                return "less than 1 km";
            }
        }


        //public async Task<IActionResult> StaticRoute(RoutingViewModel model)
        //{


        //    string x= configuration.GetValue<string>("ApiOpenWeather");
        //    string y= configuration.GetValue<string>("US1");

        //    string ReverseGeocodingKey = "pk.6a0568ea2a60f5218a864c2d9f7e5432";

        //    var httpClient1 = new HttpClient();
        //    var url1 = "https://us1.locationiq.com/v1/reverse.php?key=" + ReverseGeocodingKey + "&lat=" + model.UserLatitude + "&lon=" + model.UserLongitude + "&format=json";
        //    HttpResponseMessage response1 = await httpClient1.GetAsync(url1);

        //    string responseBody1 = await response1.Content.ReadAsStringAsync();
        //    JObject reverseGeocodingObj = JObject.Parse(responseBody1);

        //    string postCode = (string)reverseGeocodingObj["address"]["postcode"];
        //    var httpClient = new HttpClient();

        //    string OpenWeatherKey = "";


        //    var url = "http://api.openweathermap.org/data/2.5/weather?q=" + postCode + ",pl&units=metric&APPID=41270c91174b3fd8bdae41229160b95d";
        //    HttpResponseMessage response = await httpClient.GetAsync(url);

        //    string responseBody = await response.Content.ReadAsStringAsync();
        //    JObject o = JObject.Parse(responseBody);

        //    Weather_Data weather = new Weather_Data();
        //    weather.City = (string)o["name"];
        //    weather.Temp = (double)o["main"]["temp"];
        //    weather.Temp_Min = (double)o["main"]["temp_min"];
        //    weather.Temp_Max = (double)o["main"]["temp_max"];
        //    weather.Description = (string)o["weather"][0]["description"];

        //    model.details = weather;


        //    return View("StaticRoute", model);
        //}



        public async Task<IActionResult> StaticRoute(RoutingViewModel model)
        {

            string TomTomKey= configuration.GetValue<string>("TomTomKey");
            ViewBag.TomTomkey = TomTomKey; 
            string  ReverseGeocodingKey= configuration.GetValue<string>("ApiOpenWeather");
            string OpenWeatherKey = configuration.GetValue<string>("US1");          

            var httpClient1 = new HttpClient();
            var url1 = "https://us1.locationiq.com/v1/reverse.php?key=" + ReverseGeocodingKey + "&lat=" + model.UserLatitude + "&lon=" + model.UserLongitude + "&format=json";
            HttpResponseMessage response1 = await httpClient1.GetAsync(url1);

            string responseBody1 = await response1.Content.ReadAsStringAsync();
            JObject reverseGeocodingObj = JObject.Parse(responseBody1);

            string postCode = (string)reverseGeocodingObj["address"]["postcode"];
            var httpClient = new HttpClient();        


            var url = "http://api.openweathermap.org/data/2.5/weather?q=" + postCode + ",pl&units=metric&APPID="+OpenWeatherKey;
            HttpResponseMessage response = await httpClient.GetAsync(url);

            string responseBody = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(responseBody);

            Weather_Data weather = new Weather_Data();
            weather.City = (string)o["name"];
            weather.Temp = (double)o["main"]["temp"];
            weather.Temp_Min = (double)o["main"]["temp_min"];
            weather.Temp_Max = (double)o["main"]["temp_max"];
            weather.Description = (string)o["weather"][0]["description"];

            model.details = weather;


            return View("StaticRoute", model);
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
        [ValidateAntiForgeryToken]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [Authorize]
        public IActionResult ChangePhoneNumber()
        {
            ///xss test

            ////

            ChangePhoneNumberView model = new ChangePhoneNumberView();
            string Id = GetUser().Result.Id;
            model.PhoneNumber = repository.GetPhoneNumber(Id);
            model.UserId = Id;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePhoneNumber(ChangePhoneNumberView model)
        {

            bool succes = false;
            if (ModelState.IsValid)
            {
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
            else
            {

                return View(model);
            }





        }


        public IActionResult TomTom()
        {
            return View();
        }



        [Authorize]
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

            UserSettingsModel settingsmodel = new UserSettingsModel() { MainPhotoPath = details.MainPhotoPath, Name = details.User.FirstName, Surname = details.User.Surname, Likes = details.Likes, SuperLikes = details.SuperLikes, Email = details.User.Email, PhoneNumber = details.User.PhoneNumber ?? "Update", Localization = details.CityOfResidence, SearchAge = details.SearchAge, Distance = details.SearchDistance, SearchSex = details.SearchSex ?? "Male", ShowProfile = details.ShowProfile };
            settingsmodel.Coordinates.Latitude = coordinates.Latitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            settingsmodel.Coordinates.Longitude = coordinates.Longitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            settingsmodel.SetSex(details.User);

            PanelViewModel model = new PanelViewModel(detailsmodel, settingsmodel);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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


      

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddPictureAsync(IFormFile file, string PictureNumber)
        {
            string Message = "Dodanie zdjęcia nie powiodło się !!!";


            bool success = false;
             long size = 20000000;

            if (file != null&&file.Length<size)
            {
                var uploads = Path.Combine(_environment.ContentRootPath, "UserImages");
                string FilePath;
                if (file.Length > 0)
                {

                    if (Path.GetExtension(file.FileName) == ".jpg")
                    {

                        string PathText = Path.Combine(uploads, file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            FilePath = "/Home/GetPicture/" + file.FileName;
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

                          


        [Authorize]
        [HttpGet]
        public IActionResult GetPicture(string id)
        {
            string UserId = GetUser().Result.Id;
            string uploads = Path.Combine(_environment.WebRootPath, "AppPictures");
            string text = Path.Combine(uploads, "photo.png");
            var image = System.IO.File.OpenRead(text);

            if (repository.CheckPictureOwner(id, UserId)||repository.CheckIfPictureBelongsToPair(id,UserId))
            {
                uploads = Path.Combine(_environment.ContentRootPath, "UserImages");
                text = Path.Combine(uploads, id);
                image = System.IO.File.OpenRead(text);
            }

            return File(image, "image/jpeg");
        }




        //public IActionResult Test_CORS()
        //{
        //    return View();
        //}


        //public JsonResult GetData()
        //{
        //    Weather_Data wd = new Weather_Data() { Temp = 100, Temp_Max = 101, Temp_Min = 60, Description = "Jakiś tam opis" };
        //    string stringjson = JsonConvert.SerializeObject(wd);
        //    return Json(stringjson);
        //}










    }
}