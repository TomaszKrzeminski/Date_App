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




}
