using Microsoft.AspNetCore.Mvc;

namespace MundoDev.Template.Mvc.Controllers
{
    public class FormationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            ViewData["FormationId"] = id;
            return View();
        }
    }
}
