using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;


namespace DateApp.Models
{

    public interface IRepository
    {
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


    }


    public class Repository : IRepository
    {
        AppIdentityDbContext context;

        public Repository(AppIdentityDbContext ctx)
        {
            context = ctx;
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

            PictureRemover main = new MainPhotoRemove();
            PictureRemover photo1 = new Photo1Remove();
            PictureRemover photo2 = new Photo2Remove();
            PictureRemover photo3 = new Photo3Remove();

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


                    if (match.Pair=="Yes")
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
    }
}
