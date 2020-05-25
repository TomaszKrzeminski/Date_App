using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class PairController : Controller
    {

        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;

        public PairController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
        }


        public double getValue(string value)
        {
            try
            {
                return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        [Authorize]
        public IActionResult Coordinates(string Longitude, string Latitude)
        {
            string Id = userManager.GetUserId(HttpContext.User);

            double La = getValue(Latitude);
            double Lon = getValue(Longitude);


            bool check = repository.SaveCoordinates(Id, Lon, La);

            if (check)
            {
                return RedirectToRoute(new { controller = "Pair", action = "PairPanel", });
            }
            else
            {
                string Message = "Zapisanie lokalizacji nie powiodło się sprawdz przeglądarkę";
                return View("Error", Message);
            }

        }



        public PartialViewResult ShowNextMatch(string Id, string Decision)
        {
            string UserId = userManager.GetUserId(HttpContext.User);
            bool check = repository.MatchAction(Id, UserId, Decision);
            MatchView match = repository.GetMatchViews(UserId,"",true).FirstOrDefault();
            PairPartialViewModel model = new PairPartialViewModel();
            if (match!=null&&match.PairId != "" )
            {
                model.match = match;
            }
            else
            {
                model.match = new MatchView() { PairMail = "", PairId = "", PairMainPhotoPath = "" };
                
            }



            return PartialView("PairPartial", model);
        }




        public PartialViewResult  UpdateMatches()
        {
            string Id = userManager.GetUserId(HttpContext.User);
            SearchDetails details = repository.GetUserDetails(Id);
            PairOptionsViewModel options = new PairOptionsViewModel();
            AppUser user = repository.GetUser(Id);
            List<MatchView> listMatch = repository.GetMatchViews(Id, "Yes", false);
            options.list = listMatch;
            options.UserMainPhotoPath = details.MainPhotoPath;
            options.UserName = user.UserName + " " + user.Surname;

            return PartialView("PairOptionsPartial", options);
        }





        [HttpPost]
        public IActionResult GoToPair(string id)
        {
            return View();
        }




        public IActionResult PairPanel(string select = "Pair")
        {
            PairViewModel model;
            string Id = userManager.GetUserId(HttpContext.User);
            SearchDetails details = repository.GetUserDetails(Id);
            AppUser user = repository.GetUser(Id);

            if (select == "Pair")
            {
                List<Match> list = repository.GetMatches(Id);
                bool check = repository.SearchForMatches(Id);
                PairOptionsViewModel options = new PairOptionsViewModel();
                List<MatchView> listMatch = repository.GetMatchViews(Id,"Yes",false);
                options.list = listMatch;
                options.UserMainPhotoPath = details.MainPhotoPath;
                options.UserName = user.UserName + " " + user.Surname;
                PairPartialViewModel pair = new PairPartialViewModel();
                pair.match = repository.GetMatchViews(Id, "", true).FirstOrDefault();
                model = new PairViewModel(pair, options);
                model.select = select;


            }
            else
            {
                MessageOptionsViewModel pair = new MessageOptionsViewModel();
                MessageViewModel message = new MessageViewModel();
                model = new PairViewModel(message, pair);
                model.select = select;
            }

            return View(model);


        }
    }
}