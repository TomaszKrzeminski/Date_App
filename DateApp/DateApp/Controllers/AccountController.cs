using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using DateApp.Models;
using DateApp.Models.DateApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
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


        [Route("google-login")]
        public IActionResult ExternalLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            //var auth = HttpContext.AuthenticateAsync(/*IdentityConstants.ExternalScheme*/GoogleDefaults.AuthenticationScheme);
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [Route("facebook-login")]
        public IActionResult FacebookLogin()
        {
            string redirectUrl = Url.Action("FacebookResponse", "Account");
            //var auth = HttpContext.AuthenticateAsync(/*IdentityConstants.ExternalScheme*/GoogleDefaults.AuthenticationScheme);
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        [Route("facebook-response")]
        [AllowAnonymous]
        public async Task<IActionResult> FacebookResponse()
        {


            try
            {
                var result1 = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

                if (result1.Succeeded)
                {
                    var claims = result1.Principal.Identities
                        .FirstOrDefault().Claims.Select(claim => new
                        {
                            claim.Issuer,
                            claim.OriginalIssuer,
                            claim.Type,
                            claim.Value
                        });

                }


                ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ErrorLogin", "Błąd podczas logowania za pomocą facebooka");
                }


                var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
                string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
                if (result.Succeeded)
                {
                    return RedirectToAction("Panel", "Home", null);
                }
                else
                {
                    //AppUser user = new AppUser
                    //{
                    //    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    //    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                    //};
                    AppUser user = repository.GetUserWithEmail(info.Principal.FindFirst(ClaimTypes.Email).Value);
                    if (user != null)
                    {
                        //IdentityResult identResult = await userManager.CreateAsync(user);
                        //if (identResult.Succeeded)
                        //{
                        /* identResult =*/
                        await userManager.AddLoginAsync(user, info);
                        //if (identResult.Succeeded)
                        //{
                        await signInManager.SignInAsync(user, false);
                        IdentityResult r1 = userManager.RemoveFromRoleAsync(user, "NewUser").Result;
                        IdentityResult r2 = userManager.AddToRoleAsync(user, "UserRole").Result;
                        return RedirectToAction("Panel", "Home", null);
                        //}
                        //}
                    }



                }
                //CreateModel model = new CreateModel();
                //CreateModel create = new CreateModel();
                //create.Dateofbirth = new DateTime(1990, 12, 1);
                //create.Sex = "";
                //ModelState.AddModelError("Custom", "Nie posiadasz konta  zarejestruj się na DateApp ");
                //model.Email =info.Principal.FindFirst(ClaimTypes.Email).Value ?? "";
                //model.Name= info.Principal.FindFirst(ClaimTypes.Name).Value ?? ""; ;
                //model.Surname= info.Principal.FindFirst(ClaimTypes.Surname).Value ?? ""; ;

                //return View("~/Views/Admin/Create.cshtml",model);





            }
            catch (Exception ex)
            {

            }

            return View("ErrorLogin", "Problem z logowaniem za pomocą Facebooka spróbuj za pomocą loginu i hasła");
        }



        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {


            try
            {
                var result1 = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

                if (result1.Succeeded)
                {
                    var claims = result1.Principal.Identities
                        .FirstOrDefault().Claims.Select(claim => new
                        {
                            claim.Issuer,
                            claim.OriginalIssuer,
                            claim.Type,
                            claim.Value
                        });

                }


                ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ErrorLogin", "Błąd podczas logowania za pomocą google");
                }


                var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
                string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
                if (result.Succeeded)
                {
                    return RedirectToAction("Panel", "Home", null);
                }
                else
                {
                    //AppUser user = new AppUser
                    //{
                    //    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    //    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                    //};
                    AppUser user = repository.GetUserWithEmail(info.Principal.FindFirst(ClaimTypes.Email).Value);
                    if (user != null)
                    {
                        //IdentityResult identResult = await userManager.CreateAsync(user);
                        //if (identResult.Succeeded)
                        //{
                           /* identResult =*/ await userManager.AddLoginAsync(user, info);
                            //if (identResult.Succeeded)
                            //{
                                await signInManager.SignInAsync(user, false);
                        IdentityResult r1 = userManager.RemoveFromRoleAsync(user, "NewUser").Result;
                        IdentityResult r2 = userManager.AddToRoleAsync(user, "UserRole").Result;
                        return RedirectToAction("Panel", "Home", null);
                        //}
                        //}
                    }



            }
                //CreateModel model = new CreateModel();
                //CreateModel create = new CreateModel();
                //create.Dateofbirth = new DateTime(1990, 12, 1);
                //create.Sex = "";
                //ModelState.AddModelError("Custom", "Nie posiadasz konta  zarejestruj się na DateApp ");
                //model.Email =info.Principal.FindFirst(ClaimTypes.Email).Value ?? "";
                //model.Name= info.Principal.FindFirst(ClaimTypes.Name).Value ?? ""; ;
                //model.Surname= info.Principal.FindFirst(ClaimTypes.Surname).Value ?? ""; ;

            //return View("~/Views/Admin/Create.cshtml",model);





        }
            catch (Exception ex)
            {

            }

            return View("ErrorLogin", "Problem z logowaniem za pomocą Google spróbuj za pomocą loginu i hasła");
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
            LoginModel model = new LoginModel();
            model.ReturnUrl = "";
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
                    //From = new MailAddress("DateApp@gmail.com"),
                    From = new MailAddress("DateApp123Test@gmail.com"),
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
                        IdentityResult r1 = userManager.RemoveFromRoleAsync(user, "NewUser").Result;
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


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLoginCallback(LoginModel details)
        //{

        //}







        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLogin(LoginModel details)
        //{
        //    //details.ExternalLognins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //    //var Verify = _service.RecVer(details.Token);


        //    //var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = details.ReturnUrl });
        //    //var properties = signInManager.ConfigureExternalAuthenticationProperties(details.ExternalLognins.First().ToString(), redirectUrl);

        //    var properties = new AuthenticationProperties() { RedirectUri = Url.Action("FacebookResponse") };
        //    return  Challenge(properties, FacebookDefaults.AuthenticationScheme);

        //    //if (ModelState.IsValid)
        //    //{
        //    //    AppUser user = await userManager.FindByEmailAsync(details.Email);
        //    //    if (user != null)
        //    //    {
        //    //        await signInManager.SignOutAsync();

        //    //        Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, true);
        //    //        if (result.Succeeded)
        //    //        {

        //    //            if (userManager.IsInRoleAsync(user, "Administrator").Result)
        //    //            {
        //    //                return RedirectToRoute(new { controller = "Admin", action = "AdministrationPanel" });
        //    //            }

        //    //            if (userManager.IsInRoleAsync(user, "NewUser").Result)
        //    //            {
        //    //                ConfirmEmail(user);
        //    //                return View("Error", "Musisz potwierdzić swój email sprawdz go i kliknij w załączony link");
        //    //            }


        //    //            repository.CountLogin(user.Id);

        //    //            return RedirectToRoute(new { controller = "Home", action = "Panel", Id = "MyId" });
        //    //        }
        //    //        else
        //    //        {
        //    //            System.DateTimeOffset? time = user.LockoutEnd;
        //    //            if (time != null)
        //    //            {
        //    //                DateTime now = DateTime.Now;
        //    //                DateTimeOffset time2 = (DateTimeOffset)time;
        //    //                DateTimeOffset time3 = new DateTimeOffset(now);


        //    //                TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); //this timezone has an offset of +01:00:00 on this date


        //    //                DateTimeOffset T = TimeZoneInfo.ConvertTime(time2, timezone);


        //    //                if (T > time3)
        //    //                {
        //    //                    TimeSpan M = T - time3;
        //    //                    return View("Warning", new LoggingWarningViewModel(user.Email, M.Minutes, 3));
        //    //                }



        //    //            }
        //    //        }
        //    //    }
        //    //    ModelState.AddModelError(nameof(LoginModel.Email), "Nieprawidłowa nazwa użytkownika lub hasło");
        //    //}
        //    return new ChallengeResult(details.ExternalLognins.First().ToString(), properties);
        //}






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