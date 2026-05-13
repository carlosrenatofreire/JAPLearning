using Microsoft.AspNetCore.Mvc;
using MundoDev.Template.Mvc.Models;
using System.Diagnostics;

namespace MundoDev.Template.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy() => View();
        public IActionResult Terms() => View();
        public IActionResult Faq() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
