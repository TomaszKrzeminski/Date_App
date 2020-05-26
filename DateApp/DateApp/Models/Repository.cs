using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;

namespace DateApp.Models
{

    public interface IRepository
    {

        SearchDetails GetUserDetails(string UserId);
        AppUser GetUser(string UserId);
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

                    SearchDetails details = context.SearchDetails.Find(User.Details.SearchDetailsId);
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

                if (user.Details != null && user.Details.SearchDetailsId != 0)
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
            catch(Exception ex)
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

                if(list!=null&&list.Count>0)
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
    }
}
