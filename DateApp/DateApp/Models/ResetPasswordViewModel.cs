using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class ResetPasswordViewModel
    {


        public ResetPasswordViewModel()
        {
            Password = "None";
            ConfirmPassword = "None";
            Token = "None";
            Message = "";
        }

        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
       
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Potwierdzone hasło jest inne od tego z pierwszego okienka")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
