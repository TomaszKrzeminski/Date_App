﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace DateApp.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;
        private Func<Task<AppUser>> GetUser;
        private Func<HttpClient> GetClient;
        private ICitiesInRange citiesInRange;
        public EventController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env, ICitiesInRange citiesRange, Func<Task<AppUser>> GetUser = null, Func<HttpClient> GetClient = null)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
            this.citiesInRange = citiesRange;

            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }

            if (GetClient == null)
            {
                this.GetClient = () => new HttpClient();
            }
            else
            {
                this.GetClient = GetClient;
            }



        }

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

            //check if user has that event

            string ID = GetUser().Result.Id;
            bool check1 = repository.CheckIfEventBelongsToUser(EventId, ID);

            if (check1)
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
                //var httpClient1 = new HttpClient();
                var httpClient1 = GetClient();
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
                var httpClient1 = GetClient();
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

        [HttpPost]
        public IActionResult AddEvent(AddEventViewModel model)
        {
            MovieMP4 mp4 = new MovieMP4();
            PictureJPG jpg = new PictureJPG();


            if (model.Event.Date != null)
            {
                DateTime now = DateTime.Now;
                TimeSpan span = model.Event.Date - now;
                if (span.Days < 7)
                {
                    ModelState.AddModelError("Event.Date", "Możesz dodać wydażenie co najmniej  7 dni wcześniej ");
                }

            }

            if (!model.CheckExtension(model.PictureFile_1, jpg))
            {
                ModelState.AddModelError("PictureFile_1", "zdjęcie 1 nie jest typu jpg");
            }
            if (!model.CheckExtension(model.PictureFile_2, jpg))
            {
                ModelState.AddModelError("PictureFile_2", "zdjęcie 2 nie jest typu jpg");
            }
            if (!model.CheckExtension(model.PictureFile_3, jpg))
            {
                ModelState.AddModelError("PictureFile_3", "zdjęcie 3 nie jest typu jpg");
            }
            if (!model.CheckExtension(model.MovieFile, mp4))
            {
                ModelState.AddModelError("MovieFile", "film nie jest w formacie mp4");
            }

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
                return RedirectToAction("CheckEventAdding", new { EventId = eventId });
            }
            else
            {
                return View(model);
            }

        }

        public IActionResult CheckEventAdding(string EventId)
        {
            AppUser user = GetUser().Result;
            //check if user has Event with that id



            return View("CheckEventAdding",EventId);
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
                DistanceHandler distance = new DistanceHandler(repository, citiesInRange);
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

            long size = 20000000;
            if (file != null&&file.Length<size)
            {
                var uploads = Path.Combine(_environment.ContentRootPath, "EventImages");

                if (file.Length > 0)
                {

                    if (Path.GetExtension(file.FileName) == ".jpg")
                    {

                        PathText = Path.Combine(uploads, file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            FilePath = "/Event/GetPictureEvent/" + file.FileName;
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

            long size = 250000000;

            if (file != null&&file.Length<size)
            {
                var uploads = Path.Combine(_environment.ContentRootPath, "EventMovies");

                if (file.Length > 0)
                {

                    if (Path.GetExtension(file.FileName) == ".mp4")
                    {

                        PathText = Path.Combine(uploads, file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            FilePath = "/Event/GetMovieEvent/" + file.FileName;
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

        [Authorize]
        [HttpGet]
        public IActionResult GetPictureEvent(string id)
        {
            string UserId = GetUser().Result.Id;
            string uploads = Path.Combine(_environment.WebRootPath, "AppPictures");
            string text = Path.Combine(uploads, "photo.png");
            var image = System.IO.File.OpenRead(text);

            if (repository.CheckIfEventPictureBelongsToUser(id, UserId))
            {
                uploads = Path.Combine(_environment.ContentRootPath, "EventImages");
                text = Path.Combine(uploads, id);
                image = System.IO.File.OpenRead(text);
            }

            return File(image, "image/jpeg");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMovieEvent(string id)
        {
            string UserId = GetUser().Result.Id;
            string uploads = Path.Combine(_environment.WebRootPath, "AppPictures");
            string text = Path.Combine(uploads, "sampleVideo.mp4");
            var movie = System.IO.File.OpenRead(text);

            if (repository.CheckIfEventMovieBelongsToUser(id, UserId))
            {
                uploads = Path.Combine(_environment.ContentRootPath, "EventMovies");
                text = Path.Combine(uploads, id);
                movie = System.IO.File.OpenRead(text);
            }

            return File(movie, "video/mp4");
        }


    }
   



}