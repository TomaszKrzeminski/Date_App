using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using DateApp.Models;
using DateApp.Models.DateApp.Models;
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
        private GoogleCaptchaService _service;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr, IRepository repo, GoogleCaptchaService service, Func<Task<AppUser>> GetUser = null)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            repository = repo;
            _service = service;

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

        public string MakeHtmlMessage(string Email, string link)
        {



            string body = string.Format(@"


   <div>
<h1>Potwierdź konto na DateApp " + Email + @"  </h1>
</div>
<div>
<p> kliknij w ten link </p>
" + link + @"
</div>


");

            return body;

        }

        public bool SendConfirmEmail(AppUser user, string link)
        {
            try
            {
                string body = MakeHtmlMessage(user.Email, link);
                MailMessage newMail = new MailMessage
                {
                    From = new MailAddress("DateApp@gmail.com"),
                    Subject = "Potwierdzenie konta w DateApp " + DateTime.Now.ToShortDateString(),
                    Body = body,
                    IsBodyHtml = true,
                };
                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                newMail.To.Add(user.Email);
                SetSmtpClient2 Client = new SetSmtpClient2();
                SmtpClient client = Client.SetClient();


                using (client)
                {

                    client.Send(newMail);


                }



                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public void ConfirmEmail(AppUser user)
        {


            var token = userManager.GenerateEmailConfirmationTokenAsync(user).Result;

            var confirmLink = Url.Action("ConfirmAccount",
                            "Account", new { token = token, Email = user.Email },
                             protocol: HttpContext.Request.Scheme);

            SendConfirmEmail(user, confirmLink);


        }

        [HttpGet]
        public ActionResult ConfirmAccount(string Token, string Email)
        {


            if (Token != null && Email != null)
            {

                try
                {
                    AppUser user = userManager.FindByEmailAsync(Email).Result;
                    IdentityResult result = userManager.
                                ConfirmEmailAsync(user, Token).Result;


                    bool check = result.Succeeded;
                    if (check)
                    {
                      IdentityResult r1=  userManager.RemoveFromRoleAsync(user, "NewUser").Result;
                        IdentityResult r2 = userManager.AddToRoleAsync(user, "UserRole").Result;
                        return View("EmailConfirmation");
                    }
                   



                }
                catch (Exception ex)
                {
                    return View("Error", "Twój email nie został potwierdzony");
                }




            }


            return View("Error", "Twój email nie został potwierdzony");


        }








        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details)
        {

            var Verify = _service.RecVer(details.Token);


            /////turn on Capchat ///////////////////////////


            //if(!Verify.Result.success&&Verify.Result.score<=0.5)
            //{
            //    ModelState.AddModelError(string.Empty,"You probobly are a boot");
            //}


            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, true);
                    if (result.Succeeded)
                    {

                        if (userManager.IsInRoleAsync(user, "Administrator").Result)
                        {
                            return RedirectToRoute(new { controller = "Admin", action = "AdministrationPanel" });
                        }

                        if (userManager.IsInRoleAsync(user, "NewUser").Result)
                        {
                            ConfirmEmail(user);
                            return View("Error", "Musisz potwierdzić swój email sprawdz go i kliknij w załączony link");
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


                            if (T > time3)
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