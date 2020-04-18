using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class AdminController : Controller
    {

        private UserManager<AppUser> userManager;


        public AdminController(UserManager<AppUser> usrMgr)
        {
            userManager = usrMgr;
        }


        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {


            if(ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Surname=model.Surname,
                    Sex=model.Sex,
                    City=model.City,
                    Dateofbirth=model.Dateofbirth,
                    Email = model.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
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



        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}