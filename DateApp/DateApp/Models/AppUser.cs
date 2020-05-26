using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DateApp.Models
{
    public class AppUser : IdentityUser
    {

        public AppUser(string Sex)
        {

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


            coordinates = new Coordinates();
            MatchUser = new List<MatchUser>();
        }




        public AppUser()
        {
            //Pair = new List<Pair>();
            MessageUser = new List<MessageUser>();

            if(Sex== "Mężczyzna")
            {
                Details = new SearchDetails("Empty","Kobieta");
            }
            else
            {
                Details = new SearchDetails("Empty", "Mężczyzna");
            }

            
            coordinates = new Coordinates();
            MatchUser = new List<MatchUser>();
        }

        public int Age { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string City { get; set; }

        public int SearchDetailsId { get; set; }
        public SearchDetails Details { get; set; }

        public int CoordinatesId { get; set; }

        public Coordinates coordinates { get; set; }
        public IList<MatchUser> MatchUser { get; set; }

        public IList<MessageUser> MessageUser { get; set; }


    }


    public class SearchDetails
    {
        public SearchDetails()
        {



        }


        public SearchDetails(string Empty)
        {
            SearchDetailsId = 0;
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
            Likes = 20;
            SuperLikes = 1;


        }





        public SearchDetails(string Empty, string searchsex)
        {
            SearchDetailsId = 0;
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
            Likes = 20;
            SuperLikes = 1;

        }



        public int SearchDetailsId { get; set; }
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
        }


        public int MatchId { get; set; }
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



    //public class PairOptions
    //{

    //    public int  GetDistance(double Latitude,double Longitude)
    //    {
    //        return 0;
    //    }


    //    public PairOptionsViewModel GetPairs()
    //    {
    //        return new PairOptionsViewModel();
    //    }





    //}




}
