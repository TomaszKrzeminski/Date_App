﻿using System;
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

        /// Settings
        /// 



    }
    
    public class UserSettingsModel
    {

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


        public PairViewModel( MessageViewModel messageview,MessageOptionsViewModel messageOptionsview  )        
        {
            this.messageview = messageview;
            this.messageOptionsview = messageOptionsview;
            this.pairpartial = new PairPartialViewModel();
            this.pairoptions = new PairOptionsViewModel();
        }

        public PairViewModel( PairPartialViewModel pairpartial, PairOptionsViewModel pairoptions)
        {
            this.messageview = new MessageViewModel();
            this.messageOptionsview = new MessageOptionsViewModel();
            this.pairpartial = pairpartial;
            this.pairoptions = pairoptions;
        }




    }

    public class MessageOptionsViewModel
    {

    }

    public class MessageViewModel
    {

    }
    public class PairPartialViewModel
    {
      public  MatchView match { get; set; }
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

        public string PairMainPhotoPath { get; set; }
        public string PairMail { get; set; }
        public string PairId { get; set; }
       

    }

}
