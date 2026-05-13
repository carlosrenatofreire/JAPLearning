using Microsoft.AspNetCore.Mvc;

namespace MundoDev.Template.Mvc.Controllers
{
    /// <summary>
    /// Controller de template — apenas para demonstração das views.
    /// Todas as actions retornam views estáticas sem lógica de negócio.
    /// </summary>
    public class AuthController : Controller
    {
        public IActionResult Login() => View();
        public IActionResult Register() => View();
        public IActionResult ForgotPassword() => View();
    }

    public class ProjectsController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Detail(int id) => View();
    }

    public class AreasController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Detail(int id) => View();
    }

    public class PlansController : Controller
    {
        public IActionResult Index() => View();
    }

    public class ArticlesController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Detail(int id) => View();
    }

    public class AboutController : Controller
    {
        public IActionResult Index() => View();
    }

    public class AppController : Controller
    {
        public IActionResult Dashboard() => View();
        public IActionResult PersonalData() => View();
        public IActionResult ContactData() => View();
        public IActionResult Payment() => View();
        public IActionResult ChangePassword() => View();
        public IActionResult DeleteAccount() => View();
        public IActionResult MyOrders() => View();
        public IActionResult MyCertificates() => View();
        public IActionResult Player(int projectId, int lessonId) => View();
    }
}
