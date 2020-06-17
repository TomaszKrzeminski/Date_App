using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class MessageController : Controller
    {

        private IRepository repository;
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _environment;

        public MessageController(IRepository repo, UserManager<AppUser> userMgr, IHostingEnvironment env)
        {
            repository = repo;
            userManager = userMgr;
            _environment = env;
        }










        public IActionResult MessageStart(string UserId)
        {
            string Id = userManager.GetUserId(HttpContext.User);
            bool check = repository.StartChat(Id, UserId);


            return View();
        }
    }
}