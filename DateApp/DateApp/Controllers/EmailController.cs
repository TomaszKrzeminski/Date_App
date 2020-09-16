using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult SendEmailInformations()
        {
            return View();


            


        }
    }
}