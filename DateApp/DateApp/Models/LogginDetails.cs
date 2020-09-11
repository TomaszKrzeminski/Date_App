using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class LoginDetails 
    {
        public int Online { get; set; }
        public int Online_Today { get; set; }
        public int Online_ThisWeek { get; set; }
        public int Online_ThisMonth { get; set; }
        public int Users { get; set; }
        public int Users_Created_Today { get; set; }
        public int Users_Created_ThisWeek { get; set; }
        public int Users_Created_ThisMonth { get; set; }

       

    }


    public class  ShowLoginDetails:ViewComponent
    {
        private IRepository repository;
        public LoginDetails details;
        
        public ShowLoginDetails(IRepository repo)
        {
            repository = repo;
            details = repository.GetLoginDetails();
        }


 public IViewComponentResult Invoke()
        {

            return View("ShowLoginDetails",details);

        }

    }







}
