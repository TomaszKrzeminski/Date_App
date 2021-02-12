using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using System.IO;
using System.Threading.Tasks;

namespace DateApp.Controllers
{
    public class TestController : Controller
    {
        IHostingEnvironment Env;



        public TestController(IHostingEnvironment env)
        {
            Env = env;
        }

        public IActionResult Index()
        {

            ViewData["MyTomTomKey"] = "YKCJ1ZeW4GdxXOmONZi4UoSKOKpOTT4O";

            return View();
        }


        [HttpGet]
        public ActionResult SSTI()
        {
            ViewBag.Template = "Brak";
            ViewBag.Image = "test.jpg";
            return View();
        }


        [HttpPost]
        public ActionResult SSTI(string razorTpl)
        {            // WARNING This code is vulnerable on purpose: do not use in production and do not take it as an example!
            //ViewBag.RenderedTemplate = Microsoft.AspNetCore.Routing.Template.Parser(razorTpl);
            ViewBag.Template = razorTpl;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPicture(string id)
        {
            var uploads = Path.Combine(Env.ContentRootPath, "UserImages");
            string text = Path.Combine(uploads, id);
            var image = System.IO.File.OpenRead(text);
            return File(image, "image/jpeg");
        }


    }
}