using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class AdminController : Controller
    {

        private UserManager<AppUser> userManager;
        private IRepository repository;


        public AdminController(IRepository repository,UserManager<AppUser> usrMgr)
        {
            this.repository = repository;
            userManager = usrMgr;
        }


        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
                DateTime Now = DateTime.Now;                
                TimeSpan ts = Now - model.Dateofbirth;
                int age = ts.Days / 365;

            if(age<18)
            {
                ModelState.AddModelError("Custom", "Musisz mieć co najmniej 18 lat");
            }


            if(ModelState.IsValid)
            {
               



                AppUser user = new AppUser()
                {
                    Age = age,
                    UserName = model.Name,
                    Surname = model.Surname,
                    Sex = model.Sex,
                    City = model.City,
                    Dateofbirth = model.Dateofbirth,
                    Email = model.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                ///// SignalR
                //await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));


                Claim claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
                await userManager.AddClaimAsync(user,claim);
               






                if (result.Succeeded)
                {
                    return RedirectToRoute(new { controller = "Account", action = "Login", Id = "MyId" });
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }


            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Delete()
        {

            string Id = userManager.GetUserId(HttpContext.User);
            AppUser user = await userManager.FindByIdAsync(Id);
            
            if (user != null)
            {

                bool removeS = repository.RemoveSearchDetails(Id);
                bool removeC = repository.RemoveCoordinates(Id);
                bool removeM = repository.RemoveMatchesAll(Id);


                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                  return  RedirectToAction("Login", "Account", null);
                }
                else
                {
                    return View("Error", "Błąd nie można usunąć użytkownika");
                }

            }
          
                return View("Error", "Błąd nie można usunąć użytkownika");
           


        }


        


        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}