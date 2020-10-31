using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {

            ViewData["MyTomTomKey"] = "YKCJ1ZeW4GdxXOmONZi4UoSKOKpOTT4O";

            return View();
        }
    }
}