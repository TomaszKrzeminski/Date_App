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
        public IRepository repository;
        private Func<Task<AppUser>> GetUser;

        public AdminController(IRepository repository, UserManager<AppUser> usrMgr, Func<Task<AppUser>> GetUser = null)
        {
            this.repository = repository;
            userManager = usrMgr;

            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }




        }

        public PictureType GetPictureType(string PictureNumber)
        {
            PictureType type = new PictureType();
            int number;
            try
            {
                number = Convert.ToInt32(PictureNumber);
            }
            catch
            {
                number = 0;
            }


            if (number > 3 || number < 0)
            {
                number = 0;
            }
            else
            {
                type = (PictureType)number;
            }

            return type;
        }



        //public ViewResult Create()
        //{
        //    return View();
        //}



        public IActionResult AdministrationPanel()
        {
            SelectUserViewModel model = new SelectUserViewModel();

            return View(model);
        }

        public IActionResult RemoveUser(string id)
        {

            bool check = repository.RemoveUserByAdmin(id);

            if (check)
            {
                return RedirectToRoute(new { controller = "Admin", action = "AdministrationPanel" });
            }
            else
            {
                return View("Error", "Błąd przy usuwaniu użytkownika");
            }


        }

        public IActionResult AddLikes(string id)
        {
            AppUser user = repository.GetUser(id);
            int Likes = repository.GetNumberOfLikes(id);

            AddLikesViewModel model = new AddLikesViewModel();
            model.Email = user.Email;
            model.NumberOfLikes = Likes;
            model.LikesToAdd = 0;
            model.UserId = id;

            return View(model);
        }

        [HttpPost]
        public IActionResult AddLikes(AddLikesViewModel model)
        {

            bool check = repository.AddLikesByAdmin(model.UserId, model.LikesToAdd);

            if(model.LikesToAdd<=0)
            {
                ModelState.AddModelError(nameof(model.LikesToAdd),"Podaj liczbę lików");
                return View(model);
            }




            if (check)
            {
                return RedirectToRoute(new { controller = "Admin", action = "AddLikes", id = model.UserId });
            }
            else
            {
                return View("Error", "Błąd przy dodawaniu polubień");
            }



        }



        public IActionResult RemovePhoto(string id)
        {
            AppUser user = repository.GetUser(id);
            SearchDetails Details = repository.GetUserDetails(id);
            RemovePictureViewModel model = new RemovePictureViewModel();
            model.UserId = id;
            model.Email = user.Email;
            model.MainPhotoPath = Details.MainPhotoPath;
            model.PhotoPath1 = Details.PhotoPath1;
            model.PhotoPath2 = Details.PhotoPath2;
            model.PhotoPath3 = Details.PhotoPath3;

            return View(model);
        }

        [HttpPost]
        public IActionResult RemovePicture(RemovePictureViewModel model)
        {
            bool success = false;     
           
               
                PictureType type = GetPictureType(model.Number);
                success = repository.RemovePicture(model.UserId, type);           


            if (success)
            {
                return RedirectToRoute(new { controller = "Admin", action = "RemovePhoto"   ,id=model.UserId });
            }
            else
            {
                return View("Error","Nie udało się usunąć zdjęcia ");
            }


        }



        public ActionResult SearchUser(string term)
        {

            List<string> Emails = new List<string>();

            List<AppUser> list = repository.GetUsers(term);

            if (list != null)
            {
                foreach (var item in list)
                {
                    Emails.Add(item.Email);
                }
            }
            else
            {
                Emails.Add("");
            }


            //var routeList = _context.Product.Where(r => r.ProductName.Contains(term))//this is a text filter no?
            //                  .Select(r => new { id = r.ProductID, label = r.ProductName, name = "ProductNameID" }).ToArray();
            return Ok(Emails);
        }


        [HttpPost]
        public ActionResult SelectUser(string term)
        {
             SelectUserViewModel model = new SelectUserViewModel();
            if (term==null)
            {
                ModelState.AddModelError(nameof(term), "Musisz wybrać użytkownika");
                
                return View("AdministrationPanel", model);
            }
           
            AppUser user = userManager.FindByEmailAsync(term).Result;
            model.user = user;

            return View("AdministrationPanel", model);

        }



        public ActionResult Create()
        {
            CreateModel create = new CreateModel();
            return View(create);
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            DateTime Now = DateTime.Now;
            TimeSpan ts = Now - model.Dateofbirth;
            int age = ts.Days / 365;

            if (age < 18)
            {
                ModelState.AddModelError("Custom", "Musisz mieć co najmniej 18 lat");
            }


            if (ModelState.IsValid)
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
                await userManager.AddClaimAsync(user, claim);







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


            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Delete()
        {

            string Id = GetUser().Result.Id;
            AppUser user = await userManager.FindByIdAsync(Id);

            if (user != null)
            {

                //bool removeS = repository.RemoveSearchDetails(Id);
                //bool removeC = repository.RemoveCoordinates(Id);
                //bool removeM = repository.RemoveMatchesAll(Id);


                //IdentityResult result = await userManager.DeleteAsync(user);


                bool result = repository.RemoveUserByAdmin(user.Id);

                if (result)
                {
                    return RedirectToAction("Login", "Account", null);
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