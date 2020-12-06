using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DateApp.Models
{
    public class AppUser : IdentityUser
    {

        public AppUser(string Sex)
        {
            EventUser = new List<EventUser>();
            this.Sex = Sex;


            //Pair = new List<Pair>();
            MessageUser = new List<MessageUser>();

            if (Sex == "Mężczyzna")
            {
                Details = new SearchDetails("Empty", "Kobieta");
            }
            else
            {
                Details = new SearchDetails("Empty", "Mężczyzna");
            }

            ReportUsers = new List<ReportUser>();
            coordinates = new Coordinates();
            MatchUser = new List<MatchUser>();
            LoginHistory = new List<LoginHistory>();
            Notification = new NotificationCheck(false, DateTime.Now);
        }







        public AppUser()
        {
            //Pair = new List<Pair>();
            MessageUser = new List<MessageUser>();
            EventUser = new List<EventUser>();

            if (Sex == "Mężczyzna")
            {
                Details = new SearchDetails("Empty", "Kobieta");
            }
            else
            {
                Details = new SearchDetails("Empty", "Mężczyzna");
            }

            ReportUsers = new List<ReportUser>();
            coordinates = new Coordinates();
            MatchUser = new List<MatchUser>();
            LoginHistory = new List<LoginHistory>();
            Notification = new NotificationCheck(false, DateTime.Now);
        }

        public int Age { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string City { get; set; }

        public int SearchDetailsId { get; set; }
        public SearchDetails Details { get; set; }

        public int NotificationCheckId { get; set; }
        public NotificationCheck Notification { get; set; }

        public int CoordinatesId { get; set; }
        public Coordinates coordinates { get; set; }

        public IList<MatchUser> MatchUser { get; set; }
        public IList<MessageUser> MessageUser { get; set; }
        public IList<ReportUser> ReportUsers { get; set; }
        public IList<LoginHistory> LoginHistory { get; set; }
        public IList<EventUser> EventUser { get; set; }

        //public string EventId { get; set; }
        //public Event Event { get; set; }


    }


    public class EventUser
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }




    public class Event
    {

        public Event()
        {
            EventUser = new List<EventUser>();
            ZipCode = " ";
            Date = DateTime.Now;
            PhotoPath1 = "/AppPictures/photo.png";
            PhotoPath2 = "/AppPictures/photo.png";
            PhotoPath3 = "/AppPictures/photo.png";
            FilePath = "/Videos/testvideofile.mp4";
        }





        public int EventId { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy HH:MM}")]
        public DateTime Date { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string PhotoPath1 { get; set; }
        public string PhotoPath2 { get; set; }
        public string PhotoPath3 { get; set; }
        public string OrganizerEmail { get; set; }
        public IList<EventUser> EventUser { get; set; }
    }



    public class NotificationCheck
    {

        public NotificationCheck()
        {
            Check = false;
            LastCheck = new DateTime();
        }

        public NotificationCheck(bool Check, DateTime LastCheck)
        {
            this.Check = Check;
            this.LastCheck = LastCheck;
        }


        public int NotificationCheckId { get; set; }
        public bool Check { get; set; }
        public DateTime LastCheck { get; set; }

        public string AppUserId { get; set; }
        public AppUser User { get; set; }
    }



    public class ReportUser
    {
        public int ReportUserId { get; set; }
        public string Reason { get; set; }
        //Id of user that complain
        public string ComplainUserId { get; set; }


        public string ApplicationUserID { get; set; }
        public AppUser User { get; set; }

    }

    public class SearchDetails
    {
        public SearchDetails()
        {



        }


        public SearchDetails(string Empty)
        {

            UserId = "";
            SearchSex = "Kobieta";
            SearchDistance = 50;
            SearchAge = 20;
            ShowProfile = false;

            MainPhotoPath = "/AppPictures/photo.png";
            PhotoPath1 = "/AppPictures/photo.png";
            PhotoPath2 = "/AppPictures/photo.png";
            PhotoPath3 = "/AppPictures/photo.png";
            Description = "Lorem Ipsum";
            CityOfResidence = "Uzupełnij";
            Localization = "Uzupełnij";
            JobPosition = "Uzupełnij";
            CompanyName = "Uzupełnij";
            School = "Uzupełnij";
            Likes = 2;
            SuperLikes = 2;


        }

        public void SetSuperLikeDate()
        {
            SuperLikeDate = DateTime.Now.AddDays(1);
        }

        public void SetLikeDate()
        {
            LikeDate = DateTime.Now.AddDays(1);
        }

        public SearchDetails(string Empty, string searchsex)
        {

            UserId = "";
            SearchSex = searchsex;
            SearchDistance = 50;
            SearchAge = 20;
            ShowProfile = false;

            MainPhotoPath = "/AppPictures/photo.png";
            PhotoPath1 = "/AppPictures/photo.png";
            PhotoPath2 = "/AppPictures/photo.png";
            PhotoPath3 = "/AppPictures/photo.png";
            Description = "Lorem Ipsum";
            CityOfResidence = "Uzupełnij";
            Localization = "Uzupełnij";
            JobPosition = "Uzupełnij";
            CompanyName = "Uzupełnij";
            School = "Uzupełnij";
            Likes = 2;
            SuperLikes = 2;


        }



        public void ResetLike()
        {
            Likes = 2;
        }

        public void ResetSuperLike()
        {
            SuperLikes = 2;
        }

        public void ReduceLike()
        {

            bool check = CheckIfLikeIsAvailable();

            if (check)
            {
                Likes--;

                if (Likes == 0)
                {
                    SetLikeDate();
                }
            }


        }

        public void ReduceSuperLike()
        {
            bool check = CheckIfSuperLikeIsAvailable();

            if (check)
            {
                SuperLikes--;

                if (SuperLikes == 0)
                {
                    SetSuperLikeDate();
                }
            }
        }

        /// <summary>
        /// ///////        uzupełnić
        /// </summary>

        public bool CheckDateLike()
        {
            if (LikeDate <= DateTime.Now && Likes == 0)
            {
                ResetLike();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckDateSuperLike()
        {
            if (SuperLikeDate <= DateTime.Now && SuperLikes == 0)
            {
                ResetSuperLike();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool CheckIfLikeIsAvailable()
        {

            CheckDateLike();

            if (Likes > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckIfSuperLikeIsAvailable()
        {

            CheckDateSuperLike();

            if (SuperLikes > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //public int SearchDetailsId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SearchSex { get; set; }
        public int SearchDistance { get; set; }
        public int SearchAge { get; set; }
        public bool ShowProfile { get; set; }

        public string MainPhotoPath { get; set; }
        public string PhotoPath1 { get; set; }
        public string PhotoPath2 { get; set; }
        public string PhotoPath3 { get; set; }
        public string Description { get; set; }
        public string CityOfResidence { get; set; }
        public string Localization { get; set; }
        public string JobPosition { get; set; }
        public string CompanyName { get; set; }
        public string School { get; set; }
        public int Likes { get; set; }
        public int SuperLikes { get; set; }
        public DateTime SuperLikeDate { get; set; }
        public DateTime LikeDate { get; set; }

        public string AppUserId { get; set; }
        public AppUser User { get; set; }


    }


    public class MessageUser
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }
    }




    public class Message
    {

        public Message(string ReceiverId, string SenderId)
        {
            this.ReceiverId = ReceiverId;
            this.SenderId = SenderId;
            MessageText = "";
            MessageUser = new List<MessageUser>();
        }

        public Message()
        {
            MessageUser = new List<MessageUser>();
        }

        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string MessageText { get; set; }
        public DateTime Time { get; set; }
        public bool Checked { get; set; }

        public IList<MessageUser> MessageUser { get; set; }

    }


    public class MatchUser
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }
    }



    public class Match
    {

        public Match()
        {
            MatchUser = new List<MatchUser>();
            FirstUserId = "";
            SecondUserId = "";
            MainPhotoUser1 = "";
            MainPhotoUser2 = "";
            AcceptFirst = "";
            RejectFirst = "";
            SuperLikeFirst = "";
            AcceptSecond = "";
            RejectSecond = "";
            SuperLikeSecond = "";
            Pair = "";
            NewForFirstUser = true;
            NewForSecondUser = true;
        }


        public Match(string FirstId, string SecondId, string FirstPhoto, string SecondPhoto)
        {
            FirstUserId = FirstId;
            SecondUserId = SecondId;
            MainPhotoUser1 = FirstPhoto;
            MainPhotoUser2 = SecondPhoto;
            AcceptFirst = "";
            RejectFirst = "";
            SuperLikeFirst = "";
            AcceptSecond = "";
            RejectSecond = "";
            SuperLikeSecond = "";
            Pair = "";
            //Addend for notifications
            NewForFirstUser = true;
            NewForSecondUser = true;

        }


        public int MatchId { get; set; }
        public bool NewForFirstUser { get; set; }
        public bool NewForSecondUser { get; set; }
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }
        public string MainPhotoUser1 { get; set; }
        public string MainPhotoUser2 { get; set; }
        public string AcceptFirst { get; set; }
        public string RejectFirst { get; set; }
        public string SuperLikeFirst { get; set; }

        public string AcceptSecond { get; set; }
        public string RejectSecond { get; set; }
        public string SuperLikeSecond { get; set; }

        public string Pair { get; set; }
        public DateTime Time { get; set; }
        public bool DontShow { get; set; }
        public bool Reject { get; set; }
        /// <summary>
        /// Added for notifications
        /// </summary>



        public IList<MatchUser> MatchUser { get; set; }

    }


    public class Coordinates
    {

        public Coordinates()
        {

        }

        public Coordinates(string UserId, double Longitude, double Latitude)
        {
            this.UserId = UserId;
            this.Longitude = Longitude;
            this.Latitude = Latitude;
        }

        public int CoordinatesId { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }//Szerokość
        public double Longitude { get; set; }//Długość

        public string AppUserId { get; set; }
        public AppUser User { get; set; }
    }


    public class CoordinatesString
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }


    public class LoginHistory
    {

        public LoginHistory()
        {
            LoggedIn = new DateTime();
            LoggedOut = new DateTime();
        }

        public int LoginHistoryId { get; set; }
        public DateTime LoggedIn { get; set; }
        public DateTime LoggedOut { get; set; }

        public string ApplicationUserID { get; set; }
        public AppUser User { get; set; }
    }




}
