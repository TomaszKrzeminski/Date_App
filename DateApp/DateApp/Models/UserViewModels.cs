using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class CreateModel
    {
        [Required(ErrorMessage = "Podaj Imię")]
        [RegularExpression(@"^[AaĄąBbCcĆćDdEeĘęFfGgHhIiJjKkLlŁłMmNnŃńOoÓóPpRrSsŚśTtUuWwYyZzŹźŻż]*$", ErrorMessage = "Możesz używać tylko  liter")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Podaj Nazwisko")]
        [RegularExpression(@"^[AaĄąBbCcĆćDdEeĘęFfGgHhIiJjKkLlŁłMmNnŃńOoÓóPpRrSsŚśTtUuWwYyZzŹźŻż]*$", ErrorMessage = "Możesz używać tylko cyfr i liter")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Podaj Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wybierz płeć")]
        [RegularExpression(@"^[AaĄąBbCcĆćDdEeĘęFfGgHhIiJjKkLlŁłMmNnŃńOoÓóPpRrSsŚśTtUuWwYyZzŹźŻż]*$", ErrorMessage = "Możesz używać tylko cyfr i liter")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Podaj datę urodzin")]
        public DateTime Dateofbirth { get; set; }
        [Required(ErrorMessage = "Podaj Miasto")]
        [RegularExpression(@"^[AaĄąBbCcĆćDdEeĘęFfGgHhIiJjKkLlŁłMmNnŃńOoÓóPpRrSsŚśTtUuWwYyZzŹźŻż]*$", ErrorMessage = "Możesz używać tylko  liter")]
        public string City { get; set; }
        [Required]
        public string Password { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

    }

    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
        //[Required]
        public string Token { get; set; }
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


    public class RoutingViewModel
    {

        public RoutingViewModel()
        {
            details = new Weather_Data();
        }

        public RoutingViewModel(string UserLongitude, string UserLatitude, string PairLongitude, string PairLatitude)
        {
            details = new Weather_Data();
            this.UserLongitude = UserLongitude;
            this.UserLatitude = UserLatitude;
            this.PairLongitude = PairLongitude;
            this.PairLatitude = PairLatitude;
        }

        public Weather_Data details { get; set; }
        public string UserLongitude { get; set; }
        public string UserLatitude { get; set; }
        public string PairLongitude { get; set; }
        public string PairLatitude { get; set; }
    }


    public class PairDetailsViewModel
    {

        public PairDetailsViewModel()
        {
            routingViewModel = new RoutingViewModel();
        }

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

        public RoutingViewModel routingViewModel { get; set; }


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
        public string FirstName { get; set; }
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
        public bool HideProfile { get; set; }
    }

    public class ChangePhoneNumberView
    {
        [Required]
        [RegularExpression(@"^[2-9]\d{2}-\d{3}-\d{3}$", ErrorMessage = "Zły format podaj numer składający się z 9 liczb xxx-xxx-xxx")]
        public string PhoneNumber { get; set; }
        [Required]
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
            Receivers = new List<string>();
        }

        public void MakeList()
        {
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    Receivers.Add(item.ReceiverId);
                }
            }
        }

        public string ChatUserId { get; set; }
        public List<MessageShort> list;
        public List<string> Receivers;
        public string UserName { get; set; }
        public string UserMainPhotoPath { get; set; }
    }

    public class MessageViewModel
    {
        public MessageViewModel()
        {
            message = new Message();
            conversation = new List<Message>();
            info = new PageInfo();
        }
        public string UserId { get; set; }
        public string ReceiverId { get; set; }
        public string UserName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhotoPath { get; set; }
        public List<Message> conversation { get; set; }
        public Message message { get; set; }
        public PageInfo info { get; set; }
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

        public MessageShort(string ReceiverMainPhotoPath, string MessageBeggining, string Name, string ReceiverId, bool IsRead)
        {
            this.ReceiverMainPhotoPath = ReceiverMainPhotoPath;
            this.MessageBeggining = MessageBeggining;
            this.Name = Name;
            this.ReceiverId = ReceiverId;
            this.IsRead = IsRead;
        }

        public string ReceiverMainPhotoPath { get; set; }
        public string MessageBeggining { get; set; }
        public string Name { get; set; }
        public string ReceiverId { get; set; }
        public bool IsRead { get; set; }
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
            PhotoPath1 = "";
            PhotoPath2 = "";
            PhotoPath3 = "";
        }

        public string UserId { get; set; }
        public string PairMainPhotoPath { get; set; }
        public string PairMail { get; set; }
        public string PairId { get; set; }
        public MatchAction action { get; set; }
        public string PhotoPath1 { get; set; }
        public string PhotoPath2 { get; set; }
        public string PhotoPath3 { get; set; }

    }


    public class PageInfo
    {
        public int TotalMessages { get; set; }
        public int MessagesPerPage { get; set; } //5
        public int CurrentPage { get; set; }
        public string ReceiverId { get; set; }
        public int ActivePage { get; set; }
        public string Action { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalMessages / MessagesPerPage); }
        }

        public PageInfo()
        {

        }

        public PageInfo(int TotalMessages, string ReceiverId, string Action, string ActivePage, int MessagesPerPage = 5)
        {
            this.TotalMessages = TotalMessages;
            this.MessagesPerPage = MessagesPerPage;
            this.ReceiverId = ReceiverId;
            SetActivePage(ActivePage);
            SetCurrentPage(Action);
        }

        public PageInfo(int TotalMessages, string ReceiverId, int MessagesPerPage = 5)
        {
            this.TotalMessages = TotalMessages;
            this.MessagesPerPage = MessagesPerPage;
            this.ReceiverId = ReceiverId;
            int number = this.TotalPages;
            SetCurrentPage(number.ToString());
        }



        public void SetActivePage(string ActivePage)
        {
            int number;
            bool result = Int32.TryParse(ActivePage, out number);
            this.ActivePage = number;
        }


        public void SetCurrentPage(string Action)
        {

            if (Action == "Next")
            {

                int number = ActivePage + 1;

                if (number <= TotalPages)
                {
                    CurrentPage = number;
                }
                else
                {
                    CurrentPage = ActivePage;
                }

            }
            else if (Action == "Previous")
            {
                int number = ActivePage - 1;

                if (number >= 1)
                {
                    CurrentPage = number;
                }
                else
                {
                    CurrentPage = ActivePage;
                }
            }
            else
            {
                int number;
                bool result = Int32.TryParse(Action, out number);
                this.CurrentPage = number;

            }
        }




    }















}
