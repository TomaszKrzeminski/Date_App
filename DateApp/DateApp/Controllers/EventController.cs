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



        public IActionResult EventActions()
        {
            return View();
        }



        //public async Task<IActionResult> ZipCode(string code="86-105")
        //{

        //    string Key = "3fabbfd0-27e6-11eb-8826-59001fe1a22a";

        //    var httpClient1 = new HttpClient();
        //    var url1 = "https://app.zipcodebase.com/api/v1/search?apikey=" + Key + "&codes="+code;
        //    HttpResponseMessage response1 = await httpClient1.GetAsync(url1);

        //    string responseBody1 = await response1.Content.ReadAsStringAsync();
        //    JObject cityResponse = JObject.Parse(responseBody1);

        //    List <ZipCodeDetails> list = cityResponse["results"]["86-105"].ToObject<List<ZipCodeDetails>>();

        //    List<string> Cities = list.Select(c => c.city).ToList();


        //    return View("StaticRoute");
        //}

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
                Cities.AddRange( list.Select(c => c.city).ToList());
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

                codes.AddRange( list.Select(c => c.code).ToList());
            }
            catch (Exception ex)
            {

            }

            codes = codes.Distinct().ToList();

            return codes;
        }



        public IActionResult AddEvent()
        {
            AddEventViewModel model = new AddEventViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEvent(AddEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = GetUser().Result;
                model.User = user;
                repository.AddEvent(model);
                return RedirectToAction("ShowEvents");
            }
            else
            {
                return View(model);
            }

        }

        public IActionResult ShowEvent(int EventId)
        {

            EventViewModel model = new EventViewModel();

            return View(model);
        }


        public IActionResult ShowEvents()
        {
            ShowEventViewModel model = new ShowEventViewModel();

            return View("EventsSearch", model);
        }


        [HttpPost]
        public IActionResult ShowEvents(ShowEventViewModel model)
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

            return View("EventsSearch", model);
        }


        public IActionResult ShowUserEvents()
        {
            string Id = GetUser().Result.Id;
            List<Event> list = repository.GetUserEvents(Id);
            list = list.Distinct().ToList();

            return View("ShowEvents", list);
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

    }
}