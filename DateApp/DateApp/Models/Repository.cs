using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using DateApp.Models.DateApp.Models;

namespace DateApp.Models
{

    public interface IRepository
    {
        bool SetScreenShotAsMainPhoto(string Path, string UserId);
        bool CancelEvent(int EventId);
        bool CheckIfEventBelongsToUser(int EventId, string UserId);
        bool CheckIfEventPictureBelongsToUser(string picturePath, string UserId);
        bool CheckIfEventMovieBelongsToUser(string moviePath, string UserId);
        List<Event> GetEventsByZipCodes(List<string> ZipCodes);
        EventsInNeighborhoodViewModel GetEventsInNeighborhood(AppUser user, DateTime time, int Days, string ZipCode);
        List<Event> GetEventsByName(string Name);
        List<Event> GetEventsByCities(List<string> list);
        List<Event> GetEventsByCityName(string City);
        List<Event> GetEventsByDate(DateTime From, DateTime To);
        List<Event> GetUserEvents(string Id);
        Event GetEventById(int Id);
        int AddEvent(AddEventViewModel model);
        NotificationViewModel GetNotifications(string Id);
        SearchDetails GetUserDetails(string UserId);
        LoginDetails GetLoginDetails();
        AppUser GetUser(string UserId);
        List<AppUser> GetUsers(string Text);
        bool CountLogin(string Id);
        bool CountLogout(string Id);
        Task<bool> CountLogout2(string Id);
        bool ChangeUserDetails(UserDetailsModel model);
        bool AddPicture(string UserId, PictureType type, string FilePath);
        bool CheckPictureOwner(string Path, string UserId);
        bool RemovePicture(string UserId, PictureType type);
        string GetPhoneNumber(string Id);
        bool ChangePhoneNumber(string Id, string PhoneNumber);
        bool ChangeSearchSex(string Sex, string Id);
        bool SetDistance(string Id, int Distance);
        bool SetSearchAge(string UserId, int Age);
        bool SetShowProfile(string Id, bool Show);
        bool SaveCoordinates(string UserId, double Longitude, double Latitude);
        List<Match> GetMatches(string UserId);
        bool SearchForMatches(string UserId);
        List<MatchView> GetMatchViews(string UserId, string Pair, bool ExceptAfterUserAction);
        bool MatchAction(string PairId, string UserId, string Decision);
        bool RemoveSearchDetails(string Id);
        bool RemoveCoordinates(string Id);
        bool RemoveMatchesAll(string Id);
        bool RemoveMatch();
        bool ReportUser(string ComplainUserId, string UserToReport, string Reason);
        bool PairCancel(string UserId, string PairId);
        MatchAction MatchAction2(string PairId, string UserId, string Decision);
        Coordinates GetCoordinates(string UserId);
        bool StartChat(string UserId, string ReceiverId);
        List<Message> GetAllMessages(string UserId);
        List<Message> GetChat(string SenderId, string ReceiverId);
        bool SendMessage(string SenderId, string ReceiverId, string Text);
        bool ChangeMessagesToRead(string User, string SecondUser);
        bool RemoveUserByAdmin(string Id);
        bool AddLikesByAdmin(string Id, int Likes);
        int GetNumberOfLikes(string Id);
        NotificationEmail CheckPairsForNofification(string UserId);
        NotificationEmail CheckMessagesForNofification(string UserId);
        NotificationEmail CheckLikesForNotification(string UserId);
        NotificationEmail CheckSuperLikesForNofification(string UserId);
        string GetUserToNotify();
        bool SetNotify(string UserId);
        bool JoinEvent(int EventId, string UserId);

    }


    public class Repository : IRepository
    {
        AppIdentityDbContext context;
        IHostingEnvironment Environment;

        public Repository(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public Repository(AppIdentityDbContext ctx, IHostingEnvironment env)
        {
            context = ctx;
            Environment = env;
        }

        public bool MatchAction(string PairId, string UserId, string Decision)
        {
            try
            {
                bool UserIdIsFirst;
                Match match = new Match();


                if (context.Matches.Where(m => m.FirstUserId == PairId && m.SecondUserId == UserId).FirstOrDefault() != null)
                {
                    match = context.Matches.Where(m => m.FirstUserId == PairId && m.SecondUserId == UserId).First();
                    UserIdIsFirst = false;

                }
                else
                {
                    match = context.Matches.Where(m => m.FirstUserId == UserId && m.SecondUserId == PairId).First();
                    UserIdIsFirst = true;
                }

                if (Decision == "Accept")
                {
                    if (UserIdIsFirst)
                    {
                        match.AcceptFirst = "Yes";
                    }
                    else
                    {
                        match.AcceptSecond = "Yes";
                    }
                }
                else if (Decision == "Cancel")
                {
                    if (UserIdIsFirst)
                    {
                        match.RejectFirst = "Yes";
                    }
                    else
                    {
                        match.RejectSecond = "Yes";
                    }
                }
                else if (Decision == "SuperLike")
                {

                    if (UserIdIsFirst)
                    {
                        match.SuperLikeFirst = "Yes";
                    }
                    else
                    {
                        match.SuperLikeSecond = "Yes";
                    }

                }
                else
                {
                    return false;
                }
                match.Time = DateTime.Now;

                ////Check  Pair
                if (match.SuperLikeFirst == "Yes" && match.SuperLikeSecond == "Yes" || match.AcceptFirst == "Yes" && match.AcceptSecond == "Yes")
                {
                    match.Pair = "Yes";
                }
                else if (match.SuperLikeFirst == "Yes" && match.AcceptSecond == "Yes" || match.AcceptFirst == "Yes" && match.SuperLikeSecond == "Yes")
                {
                    match.Pair = "Yes";
                }
                else if (match.RejectFirst == "Yes" || match.RejectSecond == "Yes")
                {
                    match.Pair = "No";
                }


                context.SaveChanges();
                ////  check if it is a pair or not


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public MatchAction MatchAction2(string PairId, string UserId, string Decision)
        {
            MatchAction action = new MatchAction();
            action.LikeAvailable = true;
            action.SuperLikeAvailable = true;
            action.Error = false;
            action.Message = "";

            try
            {
                bool UserIdIsFirst;
                Match match = new Match();

                SearchDetails details = context.Users.Include(d => d.Details).Where(i => i.Id == UserId).First().Details;


                if (context.Matches.Where(m => m.FirstUserId == PairId && m.SecondUserId == UserId).FirstOrDefault() != null)
                {
                    match = context.Matches.Where(m => m.FirstUserId == PairId && m.SecondUserId == UserId).First();
                    UserIdIsFirst = false;

                }
                else
                {
                    match = context.Matches.Where(m => m.FirstUserId == UserId && m.SecondUserId == PairId).First();
                    UserIdIsFirst = true;
                }

                if (Decision == "Accept")
                {

                    if (details.CheckIfLikeIsAvailable())
                    {
                        if (UserIdIsFirst)
                        {
                            match.AcceptFirst = "Yes";
                        }
                        else
                        {
                            match.AcceptSecond = "Yes";
                        }

                        details.ReduceLike();

                    }
                    else
                    {
                        action.LikeAvailable = false;
                        action.SuperLikeAvailable = true;
                        action.Message = "Brak lików do " + details.LikeDate;
                    }



                }
                else if (Decision == "Cancel")
                {
                    if (UserIdIsFirst)
                    {
                        match.RejectFirst = "Yes";
                    }
                    else
                    {
                        match.RejectSecond = "Yes";
                    }
                }
                else if (Decision == "SuperLike")
                {


                    if (details.CheckIfSuperLikeIsAvailable())
                    {
                        if (UserIdIsFirst)
                        {
                            match.SuperLikeFirst = "Yes";
                        }
                        else
                        {
                            match.SuperLikeSecond = "Yes";
                        }

                        details.ReduceSuperLike();
                    }
                    else
                    {
                        action.LikeAvailable = true;
                        action.SuperLikeAvailable = false;
                        action.Message = "Brak Superlików do " + details.SuperLikeDate;
                    }




                }
                else
                {
                    action.Error = true;
                    action.LikeAvailable = false;
                    action.SuperLikeAvailable = false;
                }
                match.Time = DateTime.Now;

                ////Check  Pair
                if (match.SuperLikeFirst == "Yes" && match.SuperLikeSecond == "Yes" || match.AcceptFirst == "Yes" && match.AcceptSecond == "Yes")
                {
                    match.Pair = "Yes";
                }
                else if (match.SuperLikeFirst == "Yes" && match.AcceptSecond == "Yes" || match.AcceptFirst == "Yes" && match.SuperLikeSecond == "Yes")
                {
                    match.Pair = "Yes";
                }
                else if (match.RejectFirst == "Yes" || match.RejectSecond == "Yes")
                {
                    match.Pair = "No";
                }


                context.SaveChanges();
                ////  check if it is a pair or not


                return action;
            }
            catch (Exception ex)
            {
                action.Error = true;
                action.LikeAvailable = false;
                action.SuperLikeAvailable = false;
                action.Message = "Wystąpił nieoczekiwany błąd";
                return action;
            }


        }

        bool CheckIfExist(string UserId)
        {

            AppUser user = context.Users.Include(d => d.Details).Where(u => u.Id == UserId).First();

            if (user.Details != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool AddPicture(string UserId, PictureType type, string FilePath)
        {

            PictureSaver main = new MainPhoto();
            PictureSaver photo1 = new Photo1();
            PictureSaver photo2 = new Photo2();
            PictureSaver photo3 = new Photo3();

            main.setNumber(photo1);
            photo1.setNumber(photo2);
            photo2.setNumber(photo3);

            try
            {


                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                main.ForwardRequest(type, details, FilePath);
                context.SaveChanges();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }


        EventsInRangeDates GetWeekRange(DateTime time)
        {
            EventsInRangeDates dates = new EventsInRangeDates();
            dates.From = time;
            dates.To = time;

            int DayOFWeek = (int)time.DayOfWeek;

            if (DayOFWeek == 0)
            {
                dates.From.AddDays(-6);
                dates.To.AddDays(0);
            }

            int AddDays = 7 - DayOFWeek;
            int SubtractDays = -(DayOFWeek - 1);


            dates.To = dates.To.AddDays(AddDays);
            dates.From = dates.From.AddDays(SubtractDays);

            return dates;
        }




        public EventsInNeighborhoodViewModel GetEventsInNeighborhood(AppUser user, DateTime time, int Days, string ZipCode)
        {
            EventsInNeighborhoodViewModel model = new EventsInNeighborhoodViewModel();
            model.PostCode = ZipCode;
            DateTime To = time.AddDays(Days);
            model.Date = time;
            model.Days = Days;

            EventsInRangeDates dates = GetWeekRange(time);

            try
            {
                model.listEventsInDays = context.Events.Where(x => x.ZipCode == ZipCode && (x.Date.Date >= time.Date && x.Date.Date <= To.Date)).OrderBy(x => x.Date).ToList();
                model.listEventsInWeek = context.Events.Where(x => x.ZipCode == ZipCode && (x.Date.Date >= dates.From.Date && x.Date.Date <= dates.To.Date)).OrderBy(x => x.Date).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return model;
            }









        }

        public bool ChangePhoneNumber(string Id, string PhoneNumber)
        {

            try
            {
                AppUser user = context.Users.Where(u => u.Id == Id).First();
                user.PhoneNumber = PhoneNumber;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChangeSearchSex(string Sex, string Id)
        {

            if (Sex != null)
            {
                try
                {
                    SearchDetails model = context.Users.Include(s => s.Details).Where(u => u.Id == Id).First().Details;
                    model.SearchSex = Sex;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }





        }

        public bool ChangeUserDetails(UserDetailsModel model)
        {



            try
            {
                AppUser User = context.Users.Include(d => d.Details).Where(u => u.Id == model.UserId).First();





                if (User.Details != null)
                {

                    SearchDetails details = context.SearchDetails.Find(User.Details.Id);
                    details.MainPhotoPath = model.MainPhotoPath;
                    details.PhotoPath1 = model.PhotoPath1;
                    details.PhotoPath2 = model.PhotoPath2;
                    details.PhotoPath3 = model.PhotoPath3;
                    details.Description = model.Description;
                    details.CityOfResidence = model.CityOfResidence;
                    details.JobPosition = model.JobPosition;
                    details.CompanyName = model.CompanyName;
                    details.School = model.School;
                    details.UserId = model.UserId;

                    AppUser user = context.Users.Find(model.UserId);


                    user.Details = details;

                    context.SaveChanges();
                }
                else
                {
                    SearchDetails details = new SearchDetails();
                    details.MainPhotoPath = model.MainPhotoPath;
                    details.PhotoPath1 = model.PhotoPath1;
                    details.PhotoPath2 = model.PhotoPath2;
                    details.PhotoPath3 = model.PhotoPath3;
                    details.Description = model.Description;
                    details.CityOfResidence = model.CityOfResidence;
                    details.JobPosition = model.JobPosition;
                    details.CompanyName = model.CompanyName;
                    details.School = model.School;
                    details.UserId = model.UserId;


                    AppUser user = context.Users.Find(model.UserId);
                    user.Details = new SearchDetails()
                    {
                        MainPhotoPath = model.MainPhotoPath,
                        PhotoPath1 = model.PhotoPath1,
                        PhotoPath2 = model.PhotoPath2,
                        PhotoPath3 = model.PhotoPath3,
                        Description = model.Description,
                        CityOfResidence = model.CityOfResidence,
                        JobPosition = model.JobPosition,
                        CompanyName = model.CompanyName,
                        School = model.School,
                        UserId = model.UserId,

                    };

                    context.SaveChanges();

                }


                return true;

            }
            catch (Exception ex)
            {
                return false;

            }

        }

        public string GetPhoneNumber(string UserId)
        {
            try
            {
                string phone = context.Users.Where(u => u.Id == UserId).First().PhoneNumber;

                return phone;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SearchDetails GetUserDetails(string UserId)
        {
            try
            {
                AppUser user = context.Users.Include(s => s.Details).Where(u => u.Id == UserId).First();

                if (user.Details != null && user.Details.Id != 0)
                {

                    return user.Details;

                }
                else
                {

                    SearchDetails details = new SearchDetails("Empty");
                    user.Details = details;
                    context.SaveChanges();
                    return null;
                }



            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool RemovePicture(string UserId, PictureType type)
        {

            PictureRemover main = new MainPhotoRemove(Environment);
            PictureRemover photo1 = new Photo1Remove(Environment);
            PictureRemover photo2 = new Photo2Remove(Environment);
            PictureRemover photo3 = new Photo3Remove(Environment);

            main.setNumber(photo1);
            photo1.setNumber(photo2);
            photo2.setNumber(photo3);

            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                main.ForwardRequest(type, details);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveCoordinates(string UserId, double Longitude, double Latitude)
        {
            try
            {

                AppUser user = context.Users.Include(c => c.coordinates).Where(u => u.Id == UserId).First();

                if (user.coordinates != null && user.coordinates.CoordinatesId != 0)
                {
                    user.coordinates.Latitude = Latitude;
                    user.coordinates.Longitude = Longitude;
                    user.coordinates.UserId = UserId;
                    context.SaveChanges();

                }
                else
                {
                    Coordinates coordinates = new Coordinates();
                    coordinates.AppUserId = UserId;
                    coordinates.Latitude = Latitude;
                    coordinates.Longitude = Longitude;
                    coordinates.UserId = UserId;
                    user.coordinates = coordinates;
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetDistance(string UserId, int Distance)
        {
            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                details.SearchDistance = Distance;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        public bool SetSearchAge(string UserId, int Age)
        {
            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                details.SearchAge = Age;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        public bool SetShowProfile(string UserId, bool Show)
        {
            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                details.ShowProfile = Show;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public double MinLatitude(double Latitude, double Longitude, int Distance)
        {
            double distance;
            double change = 0.005;
            double latitude = Latitude;
            var UserCoord = new GeoCoordinate(Latitude, Longitude);

            do
            {
                var MatchCoord = new GeoCoordinate(latitude, Longitude);
                distance = UserCoord.GetDistanceTo(MatchCoord);
                latitude -= change;
            }
            while (Distance <= distance);

            return latitude;
        }

        public double MinLongitude(double Latitude, double Longitude, int Distance)
        {
            double distance;
            double change = 0.005;
            double longitude = Longitude;
            var UserCoord = new GeoCoordinate(Latitude, Longitude);

            do
            {
                var MatchCoord = new GeoCoordinate(Latitude, longitude);
                distance = UserCoord.GetDistanceTo(MatchCoord);
                longitude -= change;
            }
            while (Distance <= distance);

            return longitude;
        }

        public int GetDistance(double Latitude, double Longitude, double Latitude2, double Longitude2)
        {
            double distance;
            var UserCoord = new GeoCoordinate(Latitude, Longitude);
            var MatchCoord = new GeoCoordinate(Latitude2, Longitude2);
            distance = UserCoord.GetDistanceTo(MatchCoord);

            if (distance > 0)
            {
                distance = distance / 1000;
            }
            else
            {
                distance = 0;
            }

            return (int)distance;

        }

        public double MaxLatitude(double Latitude, double Longitude, int Distance)
        {
            double distance;
            double change = 0.005;
            double latitude = Latitude;
            var UserCoord = new GeoCoordinate(Latitude, Longitude);

            do
            {
                var MatchCoord = new GeoCoordinate(latitude, Longitude);
                distance = UserCoord.GetDistanceTo(MatchCoord);
                latitude += change;
            }
            while (Distance <= distance);

            return latitude;
        }

        public double MaxLongitude(double Latitude, double Longitude, int Distance)
        {
            double distance;
            double change = 0.005;
            double longitude = Longitude;
            var UserCoord = new GeoCoordinate(Latitude, Longitude);

            do
            {
                var MatchCoord = new GeoCoordinate(Latitude, longitude);
                distance = UserCoord.GetDistanceTo(MatchCoord);
                longitude += change;
            }
            while (Distance <= distance);

            return longitude;
        }

        public bool SearchForMatches(string UserId)
        {
            try
            {

                ///GetUser             
                AppUser user = context.Users.Include(u => u.MatchUser).Where(x => x.Id == UserId).First();
                /////


                //// Get Coordinates
                Coordinates coordinates = context.Users.Include(c => c.coordinates).Where(u => u.Id == UserId).First().coordinates;
                double Longitude = coordinates.Longitude;
                double Latitude = coordinates.Latitude;
                /////


                ////Get Search details
                SearchDetails details = context.Users.Include(d => d.Details).Where(u => u.Id == UserId).First().Details;
                string SearchSex = details.SearchSex;
                int SearchDistance = details.SearchDistance;
                int SearchAge = details.SearchAge;
                ////


                ///Get Matches
                List<MatchUser> list = context.Users.Include(x => x.MatchUser).ThenInclude(y => y.Match).Where(u => u.Id == UserId).First().MatchUser.ToList();
                List<Match> matches = list.Select(m => m.Match).ToList();
                ///

                ///Except Coordinates             

                List<string> userIdList = new List<string>();

                foreach (var m in matches)
                {
                    userIdList.Add(m.FirstUserId);
                    userIdList.Add(m.SecondUserId);
                }

                userIdList = userIdList.Distinct().ToList();


                List<Coordinates> exceptList = new List<Coordinates>();

                foreach (var u in userIdList)
                {
                    Coordinates C = context.Coordinates.Where(c => c.UserId == u).First();
                    exceptList.Add(C);
                }



                ///


                ///Filtering results

                double MaxLon = Longitude + 1;
                double MaxLat = Latitude + 1;
                double MinLat = Latitude - 1;
                double MinLon = Longitude - 1;



                List<Coordinates> listCoordinates = context.Coordinates.Include(u => u.User).ThenInclude(d => d.Details).Where(l => l.Longitude >= MinLon && l.Longitude <= MaxLon).Where(x => x.Latitude >= MinLat && x.Latitude <= MaxLat).Except(exceptList).Take(100).ToList();





                List<Coordinates> CheckList = new List<Coordinates>();



                foreach (var c in listCoordinates)
                {
                    int distance = GetDistance(Latitude, Longitude, c.Latitude, c.Longitude);
                    MatchDetails matchDetails = new MatchDetails(SearchAge, SearchDistance, SearchSex);
                    UserDetails userDetails = new UserDetails(c.User.Age, distance, c.User.Details.SearchSex);
                    SexMatch sexMatch = new SexMatch(c, CheckList);
                    AgeMatch ageMatch = new AgeMatch(c, CheckList);
                    DistanceMatch distanceMatch = new DistanceMatch(c, CheckList);

                    sexMatch.setMatch(ageMatch);
                    ageMatch.setMatch(distanceMatch);


                    sexMatch.ForwardRequest(matchDetails, userDetails);

                }





                ////MakeMatch/


                List<Match> listMatches = new List<Match>();

                foreach (var c in CheckList)
                {
                    AppUser user2 = context.Users.Include(s => s.Details).Where(i => i.Id == c.AppUserId).First();
                    SearchDetails user2SearchDetails = user2.Details;
                    listMatches.Add(new Match(UserId, c.AppUserId, details.MainPhotoPath, user2SearchDetails.MainPhotoPath));
                }



                foreach (var m in listMatches)
                {

                    context.Matches.Add(m);
                    context.SaveChanges();
                    MatchUser mU1 = new MatchUser();
                    MatchUser mU2 = new MatchUser();
                    mU1.Match = m;
                    mU1.AppUser = user;
                    AppUser secondUser = context.Users.Where(x => x.Id == m.SecondUserId).First();
                    mU2.AppUser = secondUser;
                    mU2.Match = m;

                    user.MatchUser.Add(mU1);
                    secondUser.MatchUser.Add(mU2);
                    context.SaveChanges();

                }



                /////////
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool SearchForMatches2(string UserId)
        {

            try
            {

                ///GetUser             
                AppUser user = context.Users.Include(u => u.MatchUser).Where(x => x.Id == UserId).First();
                /////


                //// Get Coordinates
                Coordinates coordinates = context.Users.Include(c => c.coordinates).Where(u => u.Id == UserId).First().coordinates;
                double Longitude = coordinates.Longitude;
                double Latitude = coordinates.Latitude;
                /////


                ////Get Search details
                SearchDetails details = context.Users.Include(d => d.Details).Where(u => u.Id == UserId).First().Details;
                string SearchSex = details.SearchSex;
                int SearchDistance = details.SearchDistance;
                int SearchAge = details.SearchAge;
                ////


                ///Get Matches
                List<MatchUser> list = context.Users.Include(x => x.MatchUser).ThenInclude(y => y.Match).Where(u => u.Id == UserId).First().MatchUser.ToList();
                List<Match> matches = list.Select(m => m.Match).ToList();
                ///

                ///Except Coordinates             

                List<string> userIdList = new List<string>();

                foreach (var m in matches)
                {
                    userIdList.Add(m.FirstUserId);
                    userIdList.Add(m.SecondUserId);
                }

                userIdList = userIdList.Distinct().ToList();


                List<Coordinates> exceptList = new List<Coordinates>();

                foreach (var u in userIdList)
                {
                    Coordinates C = context.Coordinates.Where(c => c.UserId == u).First();
                    exceptList.Add(C);
                }



                ///


                ///Filtering results

                double MaxLon = Longitude + 1;
                double MaxLat = Latitude + 1;
                double MinLat = Latitude - 1;
                double MinLon = Longitude - 1;



                List<Coordinates> listCoordinates = context.Coordinates.Include(u => u.User).Where(l => l.Longitude >= MinLon && l.Longitude <= MaxLon).Where(x => x.Latitude >= MinLat && x.Latitude <= MaxLat).Except(exceptList).Take(100).ToList();

                List<Coordinates> CheckList = new List<Coordinates>();

                foreach (var c in listCoordinates)
                {

                    int distance = GetDistance(Latitude, Longitude, c.Latitude, c.Longitude);

                    //if (distance <= SearchDistance && c.User.Age >= SearchAge && c.User.Details.SearchSex != SearchSex)
                    if (distance <= SearchDistance && c.User.Age >= SearchAge && c.User.Details.SearchSex != SearchSex)
                    {
                        CheckList.Add(c);
                    }


                }




                ////MakeMatch/


                List<Match> listMatches = new List<Match>();

                foreach (var c in CheckList)
                {
                    AppUser user2 = context.Users.Include(s => s.Details).Where(i => i.Id == c.AppUserId).First();
                    SearchDetails user2SearchDetails = user2.Details;
                    listMatches.Add(new Match(UserId, c.AppUserId, details.MainPhotoPath, user2SearchDetails.MainPhotoPath));
                }



                foreach (var m in listMatches)
                {

                    context.Matches.Add(m);
                    context.SaveChanges();
                    MatchUser mU1 = new MatchUser();
                    MatchUser mU2 = new MatchUser();
                    mU1.Match = m;
                    mU1.AppUser = user;
                    AppUser secondUser = context.Users.Where(x => x.Id == m.SecondUserId).First();
                    mU2.AppUser = secondUser;
                    mU2.Match = m;

                    user.MatchUser.Add(mU1);
                    secondUser.MatchUser.Add(mU2);
                    context.SaveChanges();

                }



                /////////
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //filterType  Yes No ""
        List<Match> IsItaPair(List<Match> matches, string filterType)
        {

            if (matches != null && matches.Count > 0)
            {
                matches = matches.Where(x => x.Pair == filterType).ToList();
            }
            else
            {
                return new List<Match>();
            }
            return matches;
        }

        //Fisrt or second user//
        List<Match> FilterMatchesUserAction(string UserType, List<Match> matches, bool Action)
        {

            if (matches != null && matches.Count > 0)
            {
                if (Action)
                {
                    if (UserType == "First")
                    {
                        matches = matches.Where(x => x.AcceptFirst == "Yes" || x.AcceptFirst == "No" || x.RejectFirst == "Yes" || x.RejectFirst == "No" || x.SuperLikeFirst == "Yes" || x.SuperLikeFirst == "No").ToList();
                    }
                    else
                    {
                        matches = matches.Where(x => x.AcceptSecond == "Yes" || x.AcceptSecond == "No" || x.RejectSecond == "Yes" || x.RejectSecond == "No" || x.SuperLikeSecond == "Yes" || x.SuperLikeSecond == "No").ToList();
                    }
                }
            }
            else
            {
                return new List<Match>();
            }
            return matches;

        }

        List<Match> ExceptAfterUserAction(string UserId, List<Match> matches)
        {

            if (matches != null && matches.Count > 0)
            {
                List<Match> list = new List<Match>();
                foreach (var M in matches)
                {
                    if (M.FirstUserId == UserId)
                    {
                        if (M.AcceptFirst == "Yes" || M.AcceptFirst == "No" || M.RejectFirst == "Yes" || M.RejectFirst == "No" || M.SuperLikeFirst == "Yes" || M.SuperLikeFirst == "No")
                        {
                            list.Add(M);
                        }
                    }
                    else
                    {
                        if (M.AcceptSecond == "Yes" || M.AcceptSecond == "No" || M.RejectSecond == "Yes" || M.RejectSecond == "No" || M.SuperLikeSecond == "Yes" || M.SuperLikeSecond == "No")
                        {
                            list.Add(M);
                        }
                    }
                }
                matches = matches.Except<Match>(list).ToList();

            }
            return matches;

        }

        public List<MatchView> GetMatchViews(string UserId, string Pair, bool ExceptUserAction)
        {

            ///////// Zmienić


            List<MatchView> list = new List<MatchView>();

            try
            {
                List<MatchUser> listmatchuser = context.Users.Include(x => x.MatchUser).ThenInclude(y => y.Match).Where(u => u.Id == UserId).First().MatchUser.ToList();
                List<Match> matches = listmatchuser.Select(m => m.Match).ToList();

                matches = IsItaPair(matches, Pair);
                if (ExceptUserAction)
                {
                    matches = ExceptAfterUserAction(UserId, matches);
                }



                ///Check if pictures are updated
                foreach (var m in matches)
                {
                    AppUser user = context.Users.Include(s => s.Details).Where(x => x.Id == m.FirstUserId).First();
                    AppUser user2 = context.Users.Include(s => s.Details).Where(x => x.Id == m.SecondUserId).First();

                    if (m.MainPhotoUser1 != user.Details.MainPhotoPath)
                    {
                        m.MainPhotoUser1 = user.Details.MainPhotoPath;
                    }
                    if (m.MainPhotoUser2 != user2.Details.MainPhotoPath)
                    {
                        m.MainPhotoUser2 = user2.Details.MainPhotoPath;
                    }
                }



                foreach (var m in matches)
                {
                    if (m.FirstUserId == UserId)
                    {
                        AppUser user = context.Users.Find(m.SecondUserId);
                        list.Add(new MatchView() { PairMail = user.Email, PairMainPhotoPath = m.MainPhotoUser2, PairId = m.SecondUserId });
                    }
                    else
                    {
                        AppUser user = context.Users.Find(m.FirstUserId);
                        list.Add(new MatchView() { PairMail = user.Email, PairMainPhotoPath = m.MainPhotoUser1, PairId = m.FirstUserId });
                    }
                }






                return list;
            }
            catch (Exception ex)
            {

                return list;

            }

        }

        public int CheckMatches(string UserId)
        {
            try
            {
                List<MatchUser> list = context.Users.Include(x => x.MatchUser).ThenInclude(y => y.Match).Where(u => u.Id == UserId).First().MatchUser.ToList();
                int matches = list.Select(m => m.Match).ToList().Count();
                return matches;

            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        public List<Match> GetMatches(string UserId)
        {
            try
            {
                List<MatchUser> list = context.Users.Include(x => x.MatchUser).ThenInclude(y => y.Match).Where(u => u.Id == UserId).First().MatchUser.ToList();
                List<Match> matches = list.Select(m => m.Match).ToList();
                return matches;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AppUser GetUser(string UserId)
        {

            try
            {
                AppUser user = context.Users.Find(UserId);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool RemoveSearchDetails(string Id)
        {
            try
            {
                AppUser user = context.Users.Include(x => x.Details).Where(z => z.Id == Id).First();
                SearchDetails details = user.Details;
                context.SearchDetails.Remove(details);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveCoordinates(string Id)
        {
            try
            {
                AppUser user = context.Users.Include(x => x.coordinates).Where(z => z.Id == Id).First();
                Coordinates coordinates = user.coordinates;
                context.Coordinates.Remove(coordinates);
                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveMatchesAll(string Id)
        {
            try
            {
                AppUser user = context.Users.Include(x => x.MatchUser).Where(z => z.Id == Id).First();
                List<MatchUser> list = user.MatchUser.ToList();

                if (list != null && list.Count > 0)
                {
                    foreach (var m in list)
                    {
                        context.MatchUsers.Remove(m);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveMatch()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ReportUser(string ComplainUserId, string UserToReport, string Reason)
        {
            try
            {
                ReportUser report = new ReportUser();
                report.Reason = Reason;
                report.ComplainUserId = ComplainUserId;
                AppUser user = context.Users.Find(UserToReport);
                user.ReportUsers.Add(report);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool PairCancel(string UserId, string PairId)
        {
            try
            {
                AppUser user = context.Users.Include(x => x.MatchUser).ThenInclude(y => y.Match).Where(u => u.Id == UserId).First();
                Match match = user.MatchUser.Where(x => x.Match.FirstUserId == PairId || x.Match.SecondUserId == PairId).First().Match;
                match.Reject = true;
                match.RejectFirst = "Yes";
                match.RejectSecond = "Yes";
                match.AcceptFirst = "No";
                match.AcceptSecond = "No";
                match.Pair = "No";
                match.Time = DateTime.Now;
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public Coordinates GetCoordinates(string UserId)
        {
            Coordinates coordinates = new Coordinates();

            try
            {
                AppUser user = context.Users.Include(x => x.coordinates).Where(y => y.Id == UserId).First();
                coordinates = user.coordinates;
                return coordinates;
            }
            catch (Exception ex)
            {
                return coordinates;
            }
        }

        public bool StartChat(string UserId, string ReceiverId)
        {
            try
            {
                AppUser user = context.Users.Where(u => u.Id == UserId).First();

                AppUser userReceiver = context.Users.Where(u => u.Id == ReceiverId).First();


                List<MatchUser> list = context.Users.Include(m => m.MatchUser).ThenInclude(me => me.Match).Where(u => u.Id == UserId).First().MatchUser.ToList();
                Match match = list.Where(u => u.Match.SecondUserId == ReceiverId || u.Match.FirstUserId == ReceiverId).First().Match;

                if (match.FirstUserId == UserId)
                {
                    match.NewForFirstUser = false;
                    context.SaveChanges();
                }
                else
                {
                    match.NewForSecondUser = false;
                    context.SaveChanges();
                }




                Message message = new Message();
                message.Checked = false;
                message.Time = DateTime.Now;
                message.SenderId = ReceiverId;
                message.ReceiverId = UserId;
                message.MessageText = "Zacznij Rozmowę " + user.UserName;

                MessageUser mu = new MessageUser();
                mu.AppUser = user;
                mu.Message = message;

                user.MessageUser.Add(mu);

                Message message2 = new Message();
                message2.Checked = false;
                message2.Time = DateTime.Now;

                message2.SenderId = UserId;
                message2.ReceiverId = ReceiverId;
                message2.MessageText = "Zacznij Rozmowę " + userReceiver.UserName;

                MessageUser mu2 = new MessageUser();
                mu2.AppUser = userReceiver;
                mu2.Message = message2;

                userReceiver.MessageUser.Add(mu2);



                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Message> GetAllMessages(string UserId)
        {
            List<Message> list = new List<Message>();
            try
            {
                List<MessageUser> listMessage = context.Users.Include(m => m.MessageUser).ThenInclude(me => me.Message).Where(u => u.Id == UserId).First().MessageUser.ToList();

                if (listMessage != null && listMessage.Count() > 0)
                {
                    list = listMessage.Select(x => x.Message).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }

        public bool SendMessage(string SenderId, string ReceiverId, string Text)
        {
            try
            {
                Message message = new Message();
                message.Checked = false;
                message.MessageText = Text;
                message.ReceiverId = ReceiverId;
                message.SenderId = SenderId;
                message.Time = DateTime.Now;

                AppUser sender = context.Users.Find(SenderId);
                AppUser receiver = context.Users.Find(ReceiverId);

                MessageUser S = new MessageUser();
                S.AppUser = sender;
                S.Message = message;

                MessageUser R = new MessageUser();
                R.AppUser = receiver;
                R.Message = message;

                sender.MessageUser.Add(S);
                receiver.MessageUser.Add(R);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Message> GetChat(string SenderId, string ReceiverId)
        {
            List<Message> list = new List<Message>();
            try
            {
                List<MessageUser> MessagesSender = context.Users.Include(m => m.MessageUser).ThenInclude(me => me.Message).Where(u => u.Id == SenderId).First().MessageUser.ToList();
                List<MessageUser> MessagesReceiver = context.Users.Include(m => m.MessageUser).ThenInclude(me => me.Message).Where(u => u.Id == ReceiverId).First().MessageUser.ToList();

                List<Message> listS = MessagesSender.Select(m => m.Message).Where(x => x.ReceiverId == ReceiverId || x.SenderId == ReceiverId).ToList();
                List<Message> listR = MessagesReceiver.Select(m => m.Message).Where(x => x.ReceiverId == SenderId || x.SenderId == SenderId).ToList();


                if (listS != null && listS.Count > 0)
                {
                    list.AddRange(listS);
                }

                if (listR != null && listR.Count > 0)
                {
                    list.AddRange(listR);
                }




                return list.GroupBy(m => m.MessageId).Select(g => g.First()).ToList();
            }
            catch (Exception ex)
            {
                return list;
            }
        }

        public bool ChangeMessagesToRead(string User, string SecondUser)
        {
            try
            {

                List<MessageUser> MessagesSender = context.Users.Include(m => m.MessageUser).ThenInclude(me => me.Message).Where(u => u.Id == User).First().MessageUser.ToList();
                List<Message> listS = MessagesSender.Select(m => m.Message).Where(x => x.ReceiverId == SecondUser || x.SenderId == SecondUser).ToList();

                listS = listS.OrderByDescending(l => l.Time).ToList();

                if (listS != null && listS.Count() > 0)
                {

                    foreach (var message in listS)
                    {

                        //if(message.Checked==false)
                        //{
                        //    message.Checked = true;
                        //}
                        //else
                        //{
                        //    break;
                        //}


                        if (message.Checked == false)
                        {
                            message.Checked = true;
                        }


                    }

                    context.SaveChanges();

                }


                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public List<AppUser> GetUsers(string Text)
        {
            try
            {

                List<AppUser> list = new List<AppUser>();
                list = context.Users.Where(u => u.Email.StartsWith(Text)).ToList();
                return list;


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool RemoveUserByAdmin(string Id)
        {
            try
            {






                Coordinates coordinates = context.Coordinates.Where(c => c.UserId == Id).FirstOrDefault();

                if (coordinates != null)
                {
                    context.Coordinates.Remove(coordinates);
                    context.SaveChanges();
                }





                SearchDetails details = context.SearchDetails.Where(d => d.AppUserId == Id).FirstOrDefault();
                context.SearchDetails.Remove(details);
                context.SaveChanges();









                AppUser user = context.Users.Include(r => r.ReportUsers).Where(i => i.Id == Id).FirstOrDefault();

                if (user.ReportUsers != null && user.ReportUsers.Count > 0)
                {

                    foreach (var report in user.ReportUsers)
                    {
                        context.ReportUsers.Remove(report);
                    }

                    context.SaveChanges();

                }

                AppUser user3 = context.Users.Include(m => m.MatchUser).Where(i => i.Id == Id).FirstOrDefault();
                if (user3.MatchUser != null && user3.MatchUser.Count > 0)
                {

                    foreach (var match in user3.MatchUser)
                    {
                        context.MatchUsers.Remove(match);
                    }

                    context.SaveChanges();

                }


                AppUser user2 = context.Users.Include(m => m.MessageUser).Where(i => i.Id == Id).FirstOrDefault();

                if (user2.MessageUser != null && user2.MessageUser.Count > 0)
                {

                    foreach (var message in user3.MessageUser)
                    {
                        context.MessageUser.Remove(message);
                    }

                    context.SaveChanges();

                }

                AppUser user4 = context.Users.Include(m => m.LoginHistory).Where(i => i.Id == Id).FirstOrDefault();

                if (user4.LoginHistory != null && user4.LoginHistory.Count > 0)
                {

                    foreach (var history in user4.LoginHistory)
                    {
                        context.LoginHistory.Remove(history);
                    }

                    context.SaveChanges();

                }








                context.Users.Remove(user);
                context.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddLikesByAdmin(string Id, int Likes)
        {

            try
            {

                SearchDetails details = context.SearchDetails.Where(d => d.AppUserId == Id).First();
                details.LikeDate = DateTime.Now;
                details.Likes += Likes;
                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {

                return false;

            }






        }

        public int GetNumberOfLikes(string Id)
        {
            try
            {

                SearchDetails details = context.SearchDetails.Where(d => d.AppUserId == Id).First();
                return details.Likes;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool CountLogin(string Id)
        {
            try
            {

                AppUser user = context.Users.Find(Id);
                LoginHistory history = new LoginHistory();
                history.LoggedIn = DateTime.Now;

                user.LoginHistory.Add(history);
                context.SaveChanges();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CountLogout(string Id)
        {
            try
            {
                AppUser user = context.Users.Include(l => l.LoginHistory).Where(u => u.Id == Id).First();
                LoginHistory history = user.LoginHistory.Last();
                history.LoggedOut = DateTime.Now;


                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CountLogout2(string Id)
        {
            try
            {
                AppUser user = context.Users.Include(l => l.LoginHistory).Where(u => u.Id == Id).First();
                LoginHistory history = user.LoginHistory.Last();
                history.LoggedOut = DateTime.Now;


                await context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetOnline()
        {
            int number = 0;
            DateTime now = DateTime.Now;
            try
            {
                number = context.Users.Include(l => l.LoginHistory).Where(x => x.LoginHistory.Count() > 0).Select((x) => new { list = x.LoginHistory.Where(z => z.LoggedOut.Year == 1 && z.LoggedIn.DayOfYear == now.DayOfYear && z.LoggedIn.Year == now.Year) }).ToList().Where(c => c.list.Count() > 0).Count();

                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }

        public int GetOnlineToday()
        {
            int number = 0;
            DateTime n = new DateTime();
            DateTime now = DateTime.Now;
            try
            {

                number = context.Users.Include(l => l.LoginHistory).Where(x => x.LoginHistory.Count() > 0).Select((x) => new { list = x.LoginHistory.Where(z => z.LoggedIn.DayOfYear == now.DayOfYear && z.LoggedIn.Year == now.Year) }).ToList().Where(c => c.list.Count() > 0).Count();
                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }

        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        }

        public static class Predicate
        {


            public static bool DatesWeek(LoginHistory history)
            {

                DateTime date1 = DateTime.Now;
                DateTime date2 = history.LoggedIn;

                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
                var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

                return d1 == d2;
            }


            public static bool CreatedWeek(DateTime d)
            {

                DateTime date1 = DateTime.Now;
                DateTime date2 = d;

                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
                var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

                return d1 == d2;
            }






        }

        Func<LoginHistory, bool> checkWeek = Predicate.DatesWeek;
        Func<DateTime, bool> checkCreated = Predicate.CreatedWeek;





        public int GetOnlineThisWeek()
        {
            int number = 0;
            DateTime now = DateTime.Now;
            try
            {



                ///// nie dotykać gotowe
                number = context.Users.Include(l => l.LoginHistory).Where(x => x.LoginHistory.Count() > 0).Select((x) => new { list = x.LoginHistory.Where(z => checkWeek(z)) }).ToList().Where(c => c.list.Count() > 0).Count();

                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }
        public int GetOnlineThisMonth()
        {
            int number = 0;
            DateTime now = DateTime.Now;
            try
            {

                number = context.Users.Include(l => l.LoginHistory).Where(x => x.LoginHistory.Count() > 0).Select((x) => new { list = x.LoginHistory.Where(z => z.LoggedIn.Month == now.Month && z.LoggedIn.Year == now.Year) }).ToList().Where(c => c.list.Count() > 0).Count();
                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }
        public int GetCreatedThisMonth()
        {
            int number = 0;
            DateTime now = DateTime.Now;

            try
            {

                number = context.Users.Include(l => l.LoginHistory).Where(x => x.LoginHistory.Count() > 0).Where(x => x.LoginHistory.Count() > 0).Select((u) => new { FirstLogin = u.LoginHistory.FirstOrDefault().LoggedIn }).Where(x => (((DateTime)x.FirstLogin).Month == now.Month) && (((DateTime)x.FirstLogin).Year == now.Year)).Count();
                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }
        public int GetCreatedThisWeek()
        {
            int number = 0;

            try
            {

                number = context.Users.Include(l => l.LoginHistory).Where(x => x.LoginHistory.Count() > 0).Select((u) => new DateTime(u.LoginHistory.FirstOrDefault().LoggedIn.Year, u.LoginHistory.FirstOrDefault().LoggedIn.Month, u.LoginHistory.FirstOrDefault().LoggedIn.Day)).Where(x => checkCreated(x)).ToList().Count();

                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }
        public int GetCreatedToday()
        {
            int number = 0;
            DateTime now = DateTime.Now;

            try
            {
                number = context.Users.Include(l => l.LoginHistory).Where(x => x.LoginHistory.Count() > 0).Select((u) => new { FirstLogin = u.LoginHistory.FirstOrDefault().LoggedIn }).Where(x => (((DateTime)x.FirstLogin).DayOfYear == now.DayOfYear) && (((DateTime)x.FirstLogin).Year == now.Year)).Count();

                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }
        public int GetAllUsers()
        {
            int number = 0;

            try
            {
                number = context.Users.Count();

                return number;
            }
            catch (Exception ex)
            {
                return number;
            }


        }

        public LoginDetails GetLoginDetails()
        {

            LoginDetails details = new LoginDetails();

            try
            {

                details.Online = GetOnline();
                details.Online_Today = GetOnlineToday();
                details.Online_ThisWeek = GetOnlineThisWeek();
                details.Online_ThisMonth = GetOnlineThisMonth();
                details.Users = GetAllUsers();
                details.Users_Created_Today = GetCreatedToday();
                details.Users_Created_ThisWeek = GetCreatedThisWeek();
                details.Users_Created_ThisMonth = GetCreatedThisMonth();


                return details;
            }
            catch (Exception ex)
            {
                return details;
            }
        }

        public NotificationViewModel GetNotifications(string Id)
        {

            NotificationViewModel model = new NotificationViewModel();


            try
            {






                List<MatchUser> ListMatchUser = context.Users.Include(m => m.MatchUser).ThenInclude(me => me.Match).Where(u => u.Id == Id).FirstOrDefault().MatchUser.ToList();
                List<Match> list = ListMatchUser.Select(m => m.Match).Where(x => x.NewForFirstUser == true || x.NewForSecondUser == true).ToList();


                List<MessageUser> ListMessageUser = context.Users.Include(m => m.MessageUser).ThenInclude(me => me.Message).Where(u => u.Id == Id).FirstOrDefault().MessageUser.ToList();
                model.NewMessages = ListMessageUser.Select(m => m.Message).Where(x => x.Checked == false).Count();


                foreach (var match in list)
                {


                    if (match.Pair == "Yes")
                    {
                        if (match.FirstUserId == Id && match.NewForFirstUser)
                        {
                            model.NewPairs++;
                        }
                        else if (match.SecondUserId == Id && match.NewForSecondUser)
                        {
                            model.NewPairs++;
                        }
                    }






                }


                AppUser user = context.Users.Include(s => s.Details).Where(u => u.Id == Id).FirstOrDefault();
                if (user.Details.LikeDate != new DateTime())
                {
                    DateTime date = user.Details.LikeDate;
                    DateTime now = DateTime.Now;
                    TimeSpan check = date - now;

                    model.NewLikes = new TimeToWait(check.Days, check.Hours, check.Minutes);
                }



                if (user.Details.SuperLikeDate != new DateTime())
                {
                    DateTime date = user.Details.SuperLikeDate;
                    DateTime now = DateTime.Now;
                    TimeSpan check = date - now;

                    model.NewSuperLikes = new TimeToWait(check.Days, check.Hours, check.Minutes);

                }






                return model;
            }
            catch (Exception ex)
            {

                return new NotificationViewModel();

            }

        }

        public bool SetNotify(string UserId)
        {
            try
            {

                AppUser user = context.Users.Include(x => x.Notification).Where(x => x.Id == UserId).First();
                NotificationCheck check = user.Notification;
                check.Check = true;
                check.LastCheck = DateTime.Now;
                context.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public string GetUserToNotify()
        {
            string Id = null;

            try
            {
                Id = context.NotificationCheck.Where(x => x.Check == false || (x.Check == true && x.LastCheck.DayOfYear < DateTime.Now.DayOfYear)).FirstOrDefault(s => !string.IsNullOrEmpty(s.AppUserId)).AppUserId;
                return Id;
            }
            catch (Exception ex)
            {
                return Id;
            }
        }

        public string CheckPhotoPath(string Path, string ReturnIfDoesNotExist = "NoUserPhoto.jpg")
        {


            if (Path == "/AppPictures/photo.png")
            {
                return ReturnIfDoesNotExist;
            }
            else
            {

                string text = Path.Replace("/AppPictures/", "");
                return text;

            }


        }

        public NotificationEmail CheckPairsForNofification(string UserId)
        {
            PairNotificationEmail data = null;

            try
            {
                List<MatchUser> list = context.Users.Include(x => x.MatchUser).ThenInclude(y => y.Match).Where(u => u.Id == UserId).First().MatchUser.ToList();
                List<Match> pairs = list.Select(m => m.Match).Where(x => x.Pair == "Yes").OrderByDescending(x => x.Time).ToList();
                List<Match> Pairs = new List<Match>();

                foreach (var match in pairs)
                {

                    if (match.FirstUserId == UserId && match.NewForFirstUser == true)
                    {
                        Pairs.Add(match);
                    }
                    else if (match.SecondUserId == UserId && match.NewForSecondUser == true)
                    {
                        Pairs.Add(match);
                    }


                }



                string UserEmail = context.Users.Find(UserId).Email;
                int count = Pairs.Count();
                Match Pair = Pairs.FirstOrDefault();
                string EmailPair = "";
                string PairPhotoPath = "";

                if (Pair != null)
                {

                    if (Pair.FirstUserId == UserId)
                    {
                        string PairId = Pair.SecondUserId;
                        AppUser PairUser = context.Users.Include(s => s.Details).Where(u => u.Id == PairId).FirstOrDefault();
                        EmailPair = PairUser.Email;
                        PairPhotoPath = CheckPhotoPath(PairUser.Details.MainPhotoPath);

                    }
                    else
                    {
                        string PairId = Pair.FirstUserId;
                        AppUser PairUser = context.Users.Include(s => s.Details).Where(u => u.Id == PairId).FirstOrDefault();
                        EmailPair = PairUser.Email;
                        PairPhotoPath = CheckPhotoPath(PairUser.Details.MainPhotoPath);
                    }


                }
                else
                {
                    //Pair = new Match();
                    //Pair.Time = new DateTime();
                    return null;
                }

                List<string> Names = new List<string>();
                Names.Add(PairPhotoPath);
                Names.Add("PairImage.jpg");

                data = new PairNotificationEmail(Environment, UserEmail, EmailPair, Pair.Time, count, Names);

                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public NotificationEmail CheckMessagesForNofification(string UserId)
        {
            MessageNotificationEmail data = null;

            try
            {

                List<MessageUser> list = context.Users.Include(x => x.MessageUser).ThenInclude(x => x.Message).Where(y => y.Id == UserId).First().MessageUser.ToList();

                List<Message> messageList = list.Select(x => x.Message).Where(x => x.Checked == false && x.ReceiverId == UserId).OrderByDescending(x => x.Time).GroupBy(m => m.SenderId).Select(g => g.First()).ToList();

                Message message = messageList.FirstOrDefault();

                string EmailSender = "";

                string PairPhotoPath = "";

                DateTime Time = new DateTime();

                int Count = 0;

                if (message != null)
                {

                    string Id = message.SenderId;
                    AppUser User = context.Users.Include(s => s.Details).Where(u => u.Id == Id).FirstOrDefault();
                    EmailSender = User.Email;
                    PairPhotoPath = CheckPhotoPath(User.Details.MainPhotoPath);
                    Time = message.Time;
                    Count = messageList.Count();
                }
                else
                {
                    return null;
                }

                string UserEmail = context.Users.Find(UserId).Email;
                int count = messageList.Count();

                List<string> Names = new List<string>();
                Names.Add(PairPhotoPath);
                Names.Add("MessagePage.jpg");

                data = new MessageNotificationEmail(Environment, UserEmail, EmailSender, Time, Count, Names);


                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public NotificationEmail CheckLikesForNotification(string UserId)
        {
            LikeNotificationEmail data = null;

            try
            {
                AppUser user = context.Users.Include(x => x.Details).Where(x => x.Id == UserId).First();
                SearchDetails details = user.Details;
                DateTime time = DateTime.Now;

                string UserEmail = user.Email;

                if (details.LikeDate < time && details.Likes == 2)
                {
                    List<string> list = new List<string>() { "LikePage.jpg" };
                    data = new LikeNotificationEmail(Environment, UserEmail, UserEmail, details.LikeDate, list);
                }

                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public NotificationEmail CheckSuperLikesForNofification(string UserId)
        {
            SuperLikeNotificationEmail data = null;

            try
            {
                AppUser user = context.Users.Include(x => x.Details).Where(x => x.Id == UserId).First();
                SearchDetails details = user.Details;
                DateTime time = DateTime.Now;

                string UserEmail = user.Email;

                if (details.SuperLikeDate < time && details.SuperLikes == 2)
                {
                    List<string> list = new List<string>() { "SuperLikePage.jpg" };
                    data = new SuperLikeNotificationEmail(Environment, UserEmail, UserEmail, details.LikeDate, list);
                }

                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public int AddEvent(AddEventViewModel model)
        {

            try
            {



                AppUser user = model.User;
                Event Event = model.Event;
                Event.OrganizerEmail = user.Email;

                EventUser eventUser = new EventUser();
                eventUser.Event = Event;
                eventUser.AppUser = user;

                user.EventUser.Add(eventUser);
                context.SaveChanges();
                int Id = eventUser.EventId;
                return Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<Event> GetUserEvents(string Id)
        {
            List<Event> list = new List<Event>();
            try
            {

                //List<EventUser> listEvents = context.Users.Include(m => m.EventUser).ThenInclude(me => me.Event).Where(u => u.Id == Id).Last().EventUser.ToList();

                //if (listEvents != null && listEvents.Count() > 0)
                //{
                //    list = listEvents.Select(x => x.Event).ToList();
                //}
                AppUser user = context.Users.Find(Id);
                list = context.Events.Where(x => x.OrganizerEmail == user.Email).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }

        }

        public bool CancelEvent(int EventId)
        {
            try
            {
                Event Event = context.Events.Find(EventId);
                context.Events.Remove(Event);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Event> GetEventsByCities(List<string> list)
        {
            List<Event> Events = new List<Event>();
            try
            {
                foreach (var item in list)
                {
                    Events.AddRange(context.Events.Include(m => m.EventUser).ThenInclude(me => me.AppUser).Where(x => x.City == item));
                }

                return Events;
            }
            catch (Exception ex)
            {
                return Events;
            }
        }

        public List<Event> GetEventsByCityName(string City)
        {
            List<Event> Events = new List<Event>();
            try
            {

                Events = context.Events.Where(x => x.City == City).ToList();

                return Events;
            }
            catch (Exception ex)
            {
                return Events;
            }
        }

        public List<Event> GetEventsByDate(DateTime From, DateTime To)
        {
            List<Event> Events = new List<Event>();
            try
            {
                Events = context.Events.Include(m => m.EventUser).ThenInclude(me => me.AppUser).Where(x => x.Date.Date >= From.Date && x.Date.Date <= To.Date).ToList();

                return Events;


            }
            catch (Exception ex)
            {
                return Events;
            }
        }

        public List<Event> GetEventsByName(string Name)
        {
            List<Event> list = new List<Event>();
            try
            {
                list = context.Events.Include(m => m.EventUser).ThenInclude(me => me.AppUser).Where(x => x.EventName == Name).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }

        public List<Event> GetEventsByZipCodes(List<string> ZipCodes)
        {
            List<Event> list = new List<Event>();
            try
            {
                foreach (var ZipCode in ZipCodes)
                {
                    list.AddRange(context.Events.Include(m => m.EventUser).ThenInclude(me => me.AppUser).Where(x => x.ZipCode == ZipCode).ToList());
                }

                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }

        public Event GetEventById(int Id)
        {
            try
            {
                Event Event = context.Events.Include(x => x.EventUser).ThenInclude(m => m.AppUser).Where(x => x.EventId == Id).First();
                return Event;
            }
            catch (Exception ex)
            {
                return new Event() { EventName = "Błąd Podczas pobierania" };
            }
        }

        public bool JoinEvent(int EventId, string UserId)
        {
            try
            {
                AppUser user = context.Users.Find(UserId);
                Event Event = context.Events.Include(x => x.EventUser).ThenInclude(y => y.AppUser).Where(x => x.EventId == EventId).First();


                if (Event.EventUser.ToList().Any(x => x.AppUser.Id == UserId))
                {
                    return false;
                }
                else
                {
                    EventUser eventUser = new EventUser();
                    eventUser.Event = Event;
                    eventUser.AppUser = user;

                    Event.EventUser.Add(eventUser);
                    context.SaveChanges();
                    return true;
                }

            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public bool SetScreenShotAsMainPhoto(string Path, string UserId)
        {
            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                details.MainPhotoPath = Path;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckPictureOwner(string Path, string UserId)
        {

            List<string> PathsList = new List<string>();
            string PathCheck = "/Home/GetPicture/" + Path;

            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                PathsList.Add(details.MainPhotoPath);
                PathsList.Add(details.PhotoPath1);
                PathsList.Add(details.PhotoPath2);
                PathsList.Add(details.PhotoPath3);

                bool check = PathsList.Any(x => x == PathCheck);

                return check;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckIfEventBelongsToUser(int EventId, string UserId)
        {
            try
            {
                bool check = false;
                Event e = context.Events.Where(x => x.EventId == EventId).First();
                AppUser user = context.Users.Find(UserId);
                if(e.OrganizerEmail==user.Email)
                {
                    check = true;
                }
                


                return check;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckIfEventPictureBelongsToUser(string picturePath, string UserId)
        {
            try
            {
                bool check = false;

                picturePath = "/Event/GetPictureEvent/" + picturePath;


                Event e = context.Events.Where(x => x.PhotoPath1 == picturePath || x.PhotoPath2 == picturePath || x.PhotoPath3 == picturePath).First();
                AppUser user = context.Users.Find(UserId);
                if (e!=null&&e.OrganizerEmail == user.Email)
                {
                    check = true;
                }



                return check;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckIfEventMovieBelongsToUser(string moviePath, string UserId)
        {
            try
            {
                bool check = false;


                moviePath = "/Event/GetMovieEvent/" + moviePath;

                Event e = context.Events.Where(x => x.FilePath==moviePath).First();
                AppUser user = context.Users.Find(UserId);
                if (e != null && e.OrganizerEmail == user.Email)
                {
                    check = true;
                }



                return check;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
