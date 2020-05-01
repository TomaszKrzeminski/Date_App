using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DateApp.Models
{
    public class AppUser : IdentityUser
    {

        public AppUser()
        {
            Pair = new List<Pair>();
            MessageUser = new List<MessageUser>();
            Details = new SearchDetails();
        }


        public string Surname { get; set; }
        public string Sex { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string City { get; set; }

        public int SearchDetailsId{get;set;}
        public SearchDetails Details { get; set; }

        public ICollection<Pair> Pair { get; set; }
        public IList<MessageUser> MessageUser { get; set; }

    }


    public class SearchDetails
    {
        public SearchDetails()
        {

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



    public class Pair
    {
        public int PairId { get; set; }
        public int MatchUserId { get; set; }
        public  DateTime TimeOfMatch{get;set;}
        public string MatchLocalization { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

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
        public string SenderId{get;set;}
        public string ReceiverId { get; set; }
        public string MessageText { get; set; }
        public DateTime Time { get; set; }
        public bool Checked { get; set; }

        public IList<MessageUser> MessageUser { get; set; }

    }




}
