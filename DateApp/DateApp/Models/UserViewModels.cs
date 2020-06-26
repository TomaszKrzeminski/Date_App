using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }

        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public DateTime Dateofbirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Password { get; set; }


    }

    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }

    public class PanelViewModel
    {
        public UserDetailsModel detailsmodel;
        public UserSettingsModel settingsmodel;

        public PanelViewModel()
        {
            detailsmodel = new UserDetailsModel();
            settingsmodel = new UserSettingsModel();
        }

        public PanelViewModel(UserDetailsModel details, UserSettingsModel settings)
        {
            detailsmodel = details;
            settingsmodel = settings;
        }


    }

    public class UserDetailsModel
    {
        public int DetailsId { get; set; }
        public string MainPhotoPath { get; set; }
        public string PhotoPath1 { get; set; }
        public string PhotoPath2 { get; set; }
        public string PhotoPath3 { get; set; }
        public string Description { get; set; }
        public string CityOfResidence { get; set; }
        public string JobPosition { get; set; }
        public string CompanyName { get; set; }
        public string School { get; set; }
        public string UserId { get; set; }

    }


    public class PairDetailsViewModel
    {
        public int DetailsId { get; set; }
        public string MainPhotoPath { get; set; }
        public string PhotoPath1 { get; set; }
        public string PhotoPath2 { get; set; }
        public string PhotoPath3 { get; set; }
        public string Description { get; set; }
        public string CityOfResidence { get; set; }
        public string JobPosition { get; set; }
        public string CompanyName { get; set; }
        public string School { get; set; }
        public string UserId { get; set; }


        public int Age { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string City { get; set; }
    }

    public class UserSettingsModel
    {


        public UserSettingsModel()
        {
            Coordinates = new CoordinatesString();
        }

        public void SetSex(AppUser user)
        {
            if (SearchSex == null)
            {
                if (user.Sex == "Male")
                {
                    SearchSex = "Female";
                }
                else
                {
                    SearchSex = "Male";
                }
            }
        }

        public CoordinatesString Coordinates { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MainPhotoPath { get; set; }
        public int Likes { get; set; }
        public int SuperLikes { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Localization { get; set; }
        public int Distance { get; set; }
        public string SearchSex { get; set; }
        public int SearchAge { get; set; }
        public bool ShowProfile { get; set; }
    }

    public class ChangePhoneNumberView
    {
        [Required]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }

    }


    //////PairController

    public class PairViewModel
    {
        public string select;
        public MessageViewModel messageview;
        public MessageOptionsViewModel messageOptionsview;
        public PairPartialViewModel pairpartial;
        public PairOptionsViewModel pairoptions;

        public PairViewModel()
        {

        }


        public PairViewModel(MessageViewModel messageview, MessageOptionsViewModel messageOptionsview)
        {
            this.messageview = messageview;
            this.messageOptionsview = messageOptionsview;
            this.pairpartial = new PairPartialViewModel();
            this.pairoptions = new PairOptionsViewModel();
        }

        public PairViewModel(PairPartialViewModel pairpartial, PairOptionsViewModel pairoptions)
        {
            this.messageview = new MessageViewModel();
            this.messageOptionsview = new MessageOptionsViewModel();
            this.pairpartial = pairpartial;
            this.pairoptions = pairoptions;
        }

    }

    public class MessageOptionsViewModel
    {
        public MessageOptionsViewModel()
        {
            list = new List<MessageShort>();
        }

        public List<MessageShort> list;
        public string UserName { get; set; }
        public string UserMainPhotoPath { get; set; }
    }

    public class MessageViewModel
    {
        public MessageViewModel()
        {
            message = new Message();
            conversation = new List<Message>();
            info = new PagingInfo();
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhotoPath { get; set; }
        public List<Message> conversation { get; set; }
        public Message message { get; set; }
        public PagingInfo info { get; set; }
    }

    public class MessageShort
    {

        public MessageShort(string ReceiverMainPhotoPath, string MessageBeggining, string Name, string ReceiverId)
        {
            this.ReceiverMainPhotoPath = ReceiverMainPhotoPath;
            this.MessageBeggining = MessageBeggining;
            this.Name = Name;
            this.ReceiverId = ReceiverId;
        }

        public string ReceiverMainPhotoPath { get; set; }
        public string MessageBeggining { get; set; }
        public string Name { get; set; }
        public string ReceiverId { get; set; }
    }


    public class PairPartialViewModel
    {

        public PairPartialViewModel()
        {
            UserCoordinates = new CoordinatesString();
            MatchCoordinates = new CoordinatesString();
            match = new MatchView();
        }
        public CoordinatesString UserCoordinates { get; set; }
        public CoordinatesString MatchCoordinates { get; set; }

        public MatchView match { get; set; }
    }
    public class PairOptionsViewModel
    {
        public PairOptionsViewModel()
        {
            list = new List<MatchView>();
        }

        public List<MatchView> list;
        public string UserName { get; set; }
        public string UserMainPhotoPath { get; set; }

    }

    public class MatchView
    {

        public MatchView()
        {
            action = new MatchAction();
            UserId = "";
            PairMail = "";
            PairId = "";
            PairMainPhotoPath = "";
        }

        public string UserId { get; set; }
        public string PairMainPhotoPath { get; set; }
        public string PairMail { get; set; }
        public string PairId { get; set; }
        public MatchAction action { get; set; }


    }
    public class PagingInfo
    {
        public int TotalMessages { get; set; }
        public int MessagesPerPage { get; set; } //5
        public int CurrentPage { get; set; }
        public string ReceiverId { get; set; }
        public int ActiveNumber { get; set; }
        public string Action { get; set; }
       

        public PagingInfo()
        {

        }

        public PagingInfo(int TotalMessages, string ReceiverId, string Action, string ActiveNumber, int MessagesPerPage = 5)
        {
            this.TotalMessages = TotalMessages;
            this.MessagesPerPage = MessagesPerPage;
            SetActiveNumber(ActiveNumber);
            SetCurrentPage(Action);
            this.ReceiverId = ReceiverId;

        }

        


        public void SetActiveNumber(string ActiveNumber)
        {
            int number;
            bool result = Int32.TryParse(ActiveNumber, out number);
            this.ActiveNumber = number;
        }


        public void SetCurrentPage(string Action)
        {
            int number;
            bool result = Int32.TryParse(Action, out number);

            if (result)
            {
                this.CurrentPage = number;
            }
            else
            {
                int check = ActiveNumber;

                if (Action == "Previous")
                {

                    check -= 1;

                }
                else if (Action == "Next")
                {
                    check += 1;

                }
                else
                {

                    CurrentPage = ActiveNumber;
                }

                if (check < 1 || check > TotalPages)
                {
                    CurrentPage = ActiveNumber;
                }
                else
                {
                    CurrentPage = check;
                }

            }

        }



        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalMessages / MessagesPerPage); }
        }
    }

}
