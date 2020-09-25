using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class NotificationsChecker : ViewComponent
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public NotificationsChecker(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public bool CheckUserIsAuthenticated()
        {

            var principal = User as ClaimsPrincipal;
            var check = User.Identity.IsAuthenticated;

            if (check)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        public IViewComponentResult Invoke()
        {

            bool check = CheckUserIsAuthenticated();
            string UserId = "";
            if (check)
            {

                try
                {
                    UserId = userManager.GetUserAsync(HttpContext.User).Result.Id;
                }
                catch (Exception ex)
                {

                }


            }



            return View("/Views/Shared/Components/NotificationsChecker/Default.cshtml", UserId);
        }





    }
}
