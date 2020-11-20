using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Xml.Linq;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json.Linq;

namespace DateApp.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private Func<Task<AppUser>> GetUser;






        public HomeController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env, Func<Task<AppUser>> GetUser = null)
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





        //public async Task<IActionResult> CalculateRoute(RoutingViewModel model)
        //{

        //    var httpClient = new HttpClient();
        //    string Coordinates = model.UserLatitude + "," + model.UserLongitude + ":" + model.PairLatitude + "," + model.UserLongitude;
        //    string Key = "YKCJ1ZeW4GdxXOmONZi4UoSKOKpOTT4O";
        //    var url = "https://api.tomtom.com/routing/1/calculateRoute/" + Coordinates + "/json?key=" + Key;
        //    HttpResponseMessage response = await httpClient.GetAsync(url);

        //    string responseBody = await response.Content.ReadAsStringAsync();
        //    JObject o = JObject.Parse(responseBody);

        //    RoutingDetails details = new RoutingDetails();
        //    string distance = (string)o["routes"]["summary"]["lengthInMeters"];

        //    //Weather_JSON_LINQ weather = new Weather_JSON_LINQ();
        //    //weather.City = (string)o["name"];
        //    //weather.Temp = (double)o["main"]["temp"];
        //    //weather.Temp_Min = (double)o["main"]["temp_min"];
        //    //weather.Temp_Max = (double)o["main"]["temp_max"];
        //    //weather.Description = (string)o["weather"][0]["description"];









        //    return PartialView("CalculateRoute", details);
        //}



            public string ChangeToMinutes(int seconds)
        {

            if(seconds>60)
            {
                return (seconds / 60).ToString()+ " minutes";
            }
            else
            {
                return "less than 1 minute";
            }
        }


        public string ChangeMetersToKM(int meters)
        {
            if(meters>1000)
            {
                return (meters / 1000).ToString()+ " km";
            }
            else
            {
                return "less than 1 km";
            }
        }



        //public async Task<IActionResult> Panel_JSON_LINQ()
        //{

        //    string ReverseGeocodingKey = "pk.6a0568ea2a60f5218a864c2d9f7e5432";

        //    var httpClient1 = new HttpClient();
        //    var url1 = "https://us1.locationiq.com/v1/reverse.php?key=" + ReverseGeocodingKey + "&lat=53.411131729515006&lon=18.451571537874628&format=json";
        //    HttpResponseMessage response1 = await httpClient1.GetAsync(url1);

        //    string responseBody1 = await response1.Content.ReadAsStringAsync();
        //    JObject reverseGeocodingObj = JObject.Parse(responseBody1);





        //    var httpClient = new HttpClient();
        //    var url = "http://api.openweathermap.org/data/2.5/weather?q=Świecie,pl&units=metric&APPID=41270c91174b3fd8bdae41229160b95d";
        //    HttpResponseMessage response = await httpClient.GetAsync(url);

        //    string responseBody = await response.Content.ReadAsStringAsync();
        //    JObject o = JObject.Parse(responseBody);

        //    Weather_Data weather = new Weather_Data();
        //    weather.City = (string)o["name"];
        //    weather.Temp = (double)o["main"]["temp"];
        //    weather.Temp_Min = (double)o["main"]["temp_min"];
        //    weather.Temp_Max = (double)o["main"]["temp_max"];
        //    weather.Description = (string)o["weather"][0]["description"];


        //    return View(weather);
        //}








        public async Task<IActionResult> StaticRoute(RoutingViewModel model)
        {





            string ReverseGeocodingKey = "pk.6a0568ea2a60f5218a864c2d9f7e5432";

            var httpClient1 = new HttpClient();
            var url1 = "https://us1.locationiq.com/v1/reverse.php?key=" + ReverseGeocodingKey + "&lat="+model.UserLatitude+"&lon="+model.UserLongitude+"&format=json";
            HttpResponseMessage response1 = await httpClient1.GetAsync(url1);

            string responseBody1 = await response1.Content.ReadAsStringAsync();
            JObject reverseGeocodingObj = JObject.Parse(responseBody1);

            string postCode = (string)reverseGeocodingObj["address"]["postcode"];




            var httpClient = new HttpClient();
            var url = "http://api.openweathermap.org/data/2.5/weather?q="+ postCode + ",pl&units=metric&APPID=41270c91174b3fd8bdae41229160b95d";
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


            //var httpClient = new HttpClient();
            //string Coordinates = model.UserLatitude + "," + model.UserLongitude + ":" + model.PairLatitude + "," + model.UserLongitude + ":53.4072518,18.4455253";
            //string Key = "YKCJ1ZeW4GdxXOmONZi4UoSKOKpOTT4O";
            //var url = "https://api.tomtom.com/routing/1/calculateRoute/" + Coordinates + "/json?key=" + Key;
            //HttpResponseMessage response = await httpClient.GetAsync(url);

            //string responseBody = await response.Content.ReadAsStringAsync();
            //JObject o = JObject.Parse(responseBody);

            //RoutingDetails details = new RoutingDetails();

            //int d=(int)o["routes"][0]["summary"]["lengthInMeters"];
            //string d1 = (string)o["routes"][0]["summary"]["arrivalTime"];
            //string d2 = (string)o["routes"][0]["summary"]["departureTime"];
            //int d3 = (int)o["routes"][0]["summary"]["travelTimeInSeconds"];

            //details.Distance = ChangeMetersToKM(d);
            //details.arrivalTime = d1;
            //details.departureTime = d2;
            //details.travelTimeInSeconds = ChangeToMinutes(d3);

            //model.details = details;


            return View("StaticRoute", model);
        }



        //public async Task<IActionResult> StaticRoute(RoutingViewModel model)
        //{

        //    var httpClient = new HttpClient();
        //    string Coordinates = model.UserLatitude + "," + model.UserLongitude + ":" + model.PairLatitude + "," + model.UserLongitude;
        //    string Key = "YKCJ1ZeW4GdxXOmONZi4UoSKOKpOTT4O";
        //    var url = "https://api.tomtom.com/routing/1/calculateRoute/" + Coordinates + "/xml?key=" + Key;
        //    XDocument document = XDocument.Load(url);


        //    RoutingDetails details = new RoutingDetails();

        //    string obj = document.Element("calculateRouteResponse").Value;

        //    //string distance= document.Element("calculateRouteResponse").Element("route").Element("summary").Element("lengthInMeters").Value;
        //    //string time= document.Element("route").Element("summary").Attribute("departureTime").Value;







        //    return View("StaticRoute", model);
        //}





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