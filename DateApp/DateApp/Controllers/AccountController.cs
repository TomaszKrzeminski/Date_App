using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IRepository repository;
        private Func<Task<AppUser>> GetUser;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr, IRepository repo, Func<Task<AppUser>> GetUser = null)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            repository = repo;

            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            string Id = GetUser().Result.Id;
            await repository.CountLogout2(Id);
            await signInManager.SignOutAsync();


            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    ///Brutal Force prevention set 4 parameter to true 
                    ///Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, true);
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, true);
                    if (result.Succeeded)
                    {

                        if (userManager.IsInRoleAsync(user, "Administrator").Result)
                        {
                            return RedirectToRoute(new { controller = "Admin", action = "AdministrationPanel" });
                        }

                        repository.CountLogin(user.Id);

                        return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
                    }
                    else
                    {
                        System.DateTimeOffset? time = user.LockoutEnd;
                        if (time != null)
                        {
                            DateTime now = DateTime.Now;
                            DateTimeOffset time2 = (DateTimeOffset)time;
                            DateTimeOffset time3 = new DateTimeOffset(now);


                            TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); //this timezone has an offset of +01:00:00 on this date

                           
                            DateTimeOffset T = TimeZoneInfo.ConvertTime(time2, timezone);


                            if (T>time3)
                            {
                                TimeSpan M = T - time3;
                                return View("Warning", new LoggingWarningViewModel(user.Email, M.Minutes, 3));
                            }



                        }
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Nieprawidłowa nazwa użytkownika lub hasło");
            }
            return View(details);
        }


    }
}