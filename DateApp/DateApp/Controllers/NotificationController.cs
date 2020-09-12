using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult CheckNotifty(string Id="Brak")
        {
            return View();
        }
    }
}