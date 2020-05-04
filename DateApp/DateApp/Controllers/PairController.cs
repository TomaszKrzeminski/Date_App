using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class PairController : Controller
    {
        public IActionResult PairPanel()
        {

            PairViewModel model = new PairViewModel();


            return View(model);
        }
    }
}