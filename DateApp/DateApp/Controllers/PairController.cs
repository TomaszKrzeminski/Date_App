using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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


        [HttpPost]
        public IActionResult PairReport(string Reason, string UserId)
        {
            string Id = userManager.GetUserId(HttpContext.User);
            bool check = repository.ReportUser(Id, UserId, Reason);

            if (check)
            {
                return RedirectToAction("GoToPair", new { @PairId = UserId });
            }
            else
            {
                return View("Error", "Problem nie można zgłosić użytkownika");
            }

        }

        public IActionResult PairCancel(string UserId)
        {
            string Id = userManager.GetUserId(HttpContext.User);
            bool check = repository.PairCancel(Id, UserId);

            if (check)
            {
                return RedirectToRoute(new { controller = "Pair", action = "PairPanel", });
            }
            else
            {
                return View("Error", "Problem z usunięciem pary");
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
            PairPartialViewModel model = new PairPartialViewModel();

            MatchAction action = repository.MatchAction2(Id, UserId, Decision);
            MatchView match = repository.GetMatchViews(UserId, "", true).FirstOrDefault();

            if (match != null && match.PairId != "")
            {
                Coordinates coordinatesUser = repository.GetCoordinates(UserId);
                Coordinates coordinatesMatch = repository.GetCoordinates(match.PairId);

                model.UserCoordinates.Latitude = coordinatesUser.Latitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
                model.UserCoordinates.Longitude = coordinatesUser.Longitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);

                model.MatchCoordinates.Latitude = coordinatesMatch.Latitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
                model.MatchCoordinates.Longitude = coordinatesMatch.Longitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);


            }



            if (match != null && match.PairId != "")
            {
                model.match = match;
                match.UserId = UserId;
                match.action = action;


            }
            else
            {
                model.match = new MatchView() { UserId = UserId, PairMail = "", PairId = "", PairMainPhotoPath = "" };

            }



            return PartialView("PairPartial", model);
        }




        public PartialViewResult UpdateMatches()
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






        public IActionResult GoToPair(string PairId)
        {
            string id = PairId;
            SearchDetails details = repository.GetUserDetails(id);
            AppUser user = repository.GetUser(id);
            if (details == null)
            {
                return RedirectToRoute(new { controller = "Pair", action = "PairPanel" });
            }

            PairDetailsViewModel detailsmodel = new PairDetailsViewModel() { DetailsId = details.Id, MainPhotoPath = details.MainPhotoPath ?? "/AppPictures/photo.png", PhotoPath1 = details.PhotoPath1 ?? "/AppPictures/photo.png", PhotoPath2 = details.PhotoPath2 ?? "/AppPictures/photo.png", PhotoPath3 = details.PhotoPath3 ?? "/AppPictures/photo.png", Description = details.Description, CityOfResidence = details.CityOfResidence, JobPosition = details.JobPosition, CompanyName = details.CompanyName, School = details.School, UserId = details.AppUserId, Age = user.Age, Name = user.UserName, Surname = user.Surname, Email = user.Email, Dateofbirth = user.Dateofbirth, City = user.City, Sex = user.Sex };

            return View("PairDetails", detailsmodel);
        }



        public IActionResult PairPanel(string select = "Pair")
        {
            PairViewModel model;
            string Id = userManager.GetUserId(HttpContext.User);
            SearchDetails details = repository.GetUserDetails(Id);
            AppUser user = repository.GetUser(Id);

            if (select == "Pair")
            {
                List<DateApp.Models.Match> list = repository.GetMatches(Id);
                bool check = repository.SearchForMatches(Id);
                PairOptionsViewModel options = new PairOptionsViewModel();
                List<MatchView> listMatch = repository.GetMatchViews(Id, "Yes", false);
                options.list = listMatch;
                options.UserMainPhotoPath = details.MainPhotoPath;
                options.UserName = user.UserName + " " + user.Surname;
                PairPartialViewModel pair = new PairPartialViewModel();
                pair.match = repository.GetMatchViews(Id, "", true).FirstOrDefault();

                if (pair.match != null && pair.match.PairId != "")
                {
                    Coordinates coordinatesUser = repository.GetCoordinates(Id);
                    Coordinates coordinatesMatch = repository.GetCoordinates(pair.match.PairId);

                    pair.UserCoordinates.Latitude = coordinatesUser.Latitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
                    pair.UserCoordinates.Longitude = coordinatesUser.Longitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);

                    pair.MatchCoordinates.Latitude = coordinatesMatch.Latitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
                    pair.MatchCoordinates.Longitude = coordinatesMatch.Longitude.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);


                }


                if (pair.match != null)
                {
                    pair.match.UserId = Id;
                    pair.match.action = new MatchAction("", true, true, false);
                }


                model = new PairViewModel(pair, options);
                model.select = select;


            }
            else
            {
                MessageOptionsViewModel messagesOptionsView = new MessageOptionsViewModel();
                List<Message> listOfMessages = repository.GetAllMessages(Id);
                listOfMessages = listOfMessages.OrderByDescending(x => x.Time).ToList();
                ///// Remove Repetings

                listOfMessages = listOfMessages.GroupBy(x =>new { x.SenderId, x.ReceiverId }).Select(y => y.First()).ToList();
               

                /////

                List<MessageShort> shortList = new List<MessageShort>();

                foreach (var m in listOfMessages)
                {
                  SearchDetails Details = repository.GetUserDetails(m.ReceiverId);                 
                    string PhotoPath = Details.MainPhotoPath;
                    string Name = Details.User.UserName;
                    string Text = m.MessageText;
                    string ShortText="";
                    if(Text!=null&&Text.Count()>0)
                    {
                      ShortText= Regex.Replace(Text.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                        ShortText += "...";
                    }


                    shortList.Add(new MessageShort(PhotoPath,ShortText,Name,Details.User.Id));
                }

                messagesOptionsView.list = shortList;
                messagesOptionsView.UserMainPhotoPath = details.MainPhotoPath;
                messagesOptionsView.UserName = user.UserName + " " + user.Surname;
                
                ////////

                MessageViewModel messageView = new MessageViewModel();      
                

                model = new PairViewModel(messageView, messagesOptionsView);
                model.select = select;
            }

            return View(model);


        }

    }
}