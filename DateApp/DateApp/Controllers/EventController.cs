using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DateApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DateApp.Controllers
{

    public class EventController : Controller
    {


        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private Func<Task<AppUser>> GetUser;






        public EventController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env, Func<Task<AppUser>> GetUser = null)
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

        //public IActionResult EventsInNeighborhood(int Days=10)
        //{
        //    AppUser user = GetUser().Result;
        //    DateTime Date = DateTime.Now;
        //    string ZipCode = "86-100";
        //    EventsInNeighborhoodViewModel model = repository.GetEventsInNeighborhood( user, Date,Days,ZipCode);
        //    return PartialView(model);
        //}


        public IActionResult ShowEvents()
        {
            ShowEventViewModel model = new ShowEventViewModel();

            return View("EventsSearch", model);
        }

        public IActionResult ShowEvent(int EventId)
        {

            EventViewModel model = new EventViewModel();
            model.Event = repository.GetEventById(EventId);
            return View(model);
        }

        public IActionResult AddEvent()
        {
            AddEventViewModel model = new AddEventViewModel();
            model.Event.City = "Brak";
            model.Event.Description = "Brak";
            model.Event.EventName = "Brak";
            model.Event.ZipCode = "86-100";
            model.Event.PhotoPath1 = "2.jpg";
            model.Event.PhotoPath2 = "3.jpg";
            model.Event.PhotoPath3 = "4.jpg";
            model.Event.Date = DateTime.Now;

            return View(model);
        }

        public IActionResult JoinEvent(int EventId)
        {
            AppUser user = GetUser().Result;
            bool check = repository.JoinEvent(EventId, user.Id);

            if (check)
            {
                return RedirectToAction("ShowEvent", new { EventId = EventId });
            }
            else
            {
                return View("Error", "Nie udało się dołączyć do wydarzenia nieznany błąd");
            }


        }

        public IActionResult ShowUserEvents()
        {
            string Id = GetUser().Result.Id;
            List<Event> list = repository.GetUserEvents(Id);
            list = list.Distinct().ToList();

            return View("ShowEvents_View", list);
        }

        public IActionResult CancelEvent()
        {
            string Id = GetUser().Result.Id;
            List<Event> list = repository.GetUserEvents(Id);
            return View(list);
        }

        [HttpPost]
        public IActionResult CancelEvent(int EventId)
        {

            bool check = repository.CancelEvent(EventId);

            if (check)
            {
                return RedirectToAction("EventActions");
            }
            else
            {
                return View("Error", "Nie udało się usunąć wydarzenia");
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

        public IActionResult EventActions()
        {
            EventsInNeighborhoodViewModel model = new EventsInNeighborhoodViewModel();

            try
            {
                AppUser user = GetUser().Result;
                string UserId = user.Id;
                DateApp.Models.Coordinates c = repository.GetCoordinates(UserId);
                string key = "YKCJ1ZeW4GdxXOmONZi4UoSKOKpOTT4O";
                var httpClient1 = new HttpClient();
                var url = "https://api.tomtom.com/search/2/reverseGeocode/" + c.Latitude.ToString().Replace(",", ".") + "%2C" + c.Longitude.ToString().Replace(",", ".") + "+.json?key=" + key;
                HttpResponseMessage response1 = httpClient1.GetAsync(url).Result;
                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                JObject ZipCodeResponse = JObject.Parse(responseBody1);

                string ZipCode = (string)ZipCodeResponse["addresses"][0]["address"]["postalCode"];
                DateTime Date = DateTime.Now;
                model = repository.GetEventsInNeighborhood(user, Date, 10, ZipCode);
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }

        }

        [HttpGet]
        public JsonResult ZipCode(string fetch)
        {
            List<string> Cities = new List<string>();

            try
            {
                string Key = "3fabbfd0-27e6-11eb-8826-59001fe1a22a";
                var httpClient1 = new HttpClient();
                var url1 = "https://app.zipcodebase.com/api/v1/search?apikey=" + Key + "&codes=" + fetch;
                HttpResponseMessage response1 = httpClient1.GetAsync(url1).Result;
                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                JObject cityResponse = JObject.Parse(responseBody1);

                JEnumerable<JToken> Object = cityResponse["results"].First().Children();
                List<JToken> obj = AsyncEnumerable.ToAsyncEnumerable(Object).ToList().Result;
                JToken elem = obj.First();
                List<ZipCodeDetails> list = elem.ToObject<List<ZipCodeDetails>>();
                Cities.AddRange(list.Select(c => c.city).ToList());
            }
            catch (Exception ex)
            {

            }

            return Json(Cities);
        }

        public List<string> CitiesInRange(string ZipCode = "86-100", int Distance = 10)
        {

            List<string> codes = new List<string>();
            try
            {

                string Key = "3fabbfd0-27e6-11eb-8826-59001fe1a22a";
                var httpClient1 = new HttpClient();

                var url = "https://app.zipcodebase.com/api/v1/radius?apikey=" + Key + "&code=" + ZipCode + "&radius=" + Distance + "&country=pl";
                HttpResponseMessage response1 = httpClient1.GetAsync(url).Result;
                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                JObject cityResponse = JObject.Parse(responseBody1);


                List<ZipDistanceDetails> list = cityResponse["results"].ToObject<List<ZipDistanceDetails>>();

                codes.AddRange(list.Select(c => c.code).ToList());
            }
            catch (Exception ex)
            {

            }

            codes = codes.Distinct().ToList();

            return codes;
        }



        [HttpPost]
        public IActionResult AddEvent(AddEventViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.PictureFile_1 != null)
                {
                    model.Event.PhotoPath1 = AddPictureEvent(model.PictureFile_1).Result;
                }
                if (model.PictureFile_2 != null)
                {
                    model.Event.PhotoPath2 = AddPictureEvent(model.PictureFile_2).Result;
                }
                if (model.PictureFile_3 != null)
                {
                    model.Event.PhotoPath3 = AddPictureEvent(model.PictureFile_3).Result;
                }
                if (model.MovieFile != null)
                {
                    model.Event.FilePath = AddMovieFileEvent(model.MovieFile).Result;
                }

                AppUser user = GetUser().Result;
                model.User = user;
                int eventId = repository.AddEvent(model);
                return RedirectToAction("ShowEvent", new { EventId = eventId });
            }
            else
            {
                return View(model);
            }

        }







        [HttpPost]
        public IActionResult ShowEvents(ShowEventViewModel model)
        {

            if (ModelState.IsValid)
            {
                string Id = GetUser().Result.Id;
                model.UserId = Id;
                NameHandler name = new NameHandler(repository);
                DateHandler date = new DateHandler(repository);
                UserHandler user = new UserHandler(repository);
                CityNameHandler city = new CityNameHandler(repository);
                ZipCodeHandler zipcode = new ZipCodeHandler(repository);
                DistanceHandler distance = new DistanceHandler(repository);
                name.SetNext(zipcode).SetNext(distance).SetNext(date).SetNext(city).SetNext(user);
                name.Handle(model);
                model.list = model.list.Distinct().ToList();
                return View("EventsSearch", model);
            }
            else
            {
                return View("EventsSearch", model);
            }




        }







        public async Task<string> AddPictureEvent(IFormFile file)
        {
            string FilePath = "";
            string PathText = "";

            if (file != null)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "Images");

                if (file.Length > 0)
                {

                    if (Path.GetExtension(file.FileName) == ".jpg")
                    {

                        PathText = Path.Combine(uploads, file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            FilePath = "/Images/" + file.FileName;
                            await file.CopyToAsync(fileStream);
                        }


                    }

                }


                return FilePath;



            }
            else
            {
                return FilePath;
            }


        }

        public async Task<string> AddMovieFileEvent(IFormFile file)
        {

            string PathText = "";
            string FilePath = "";
            if (file != null)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "Videos");

                if (file.Length > 0)
                {

                    if (Path.GetExtension(file.FileName) == ".mp4")
                    {

                        PathText = Path.Combine(uploads, file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            FilePath = "/Videos/" + file.FileName;
                            await file.CopyToAsync(fileStream);
                        }


                    }

                }


                return FilePath;



            }
            else
            {
                return FilePath;
            }


        }





    }



}