using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Mvc.Models;
using System.Diagnostics;

namespace MundoDev.Mvc.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly IOrderService _orderService;

        public HomeController(IUserService userService, ICourseService courseService,
            IOrderService orderService, INotificator notificator) : base(notificator)
        {
            _userService = userService;
            _courseService = courseService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "dashboard";

            if (User.IsInRole("Administrador") || User.IsInRole("Supervisor"))
            {
                var users   = await _userService.GetAllAsync();
                var courses = await _courseService.GetAllAsync();
                var orders  = await _orderService.GetAllAsync();

                ViewBag.TotalUsers   = users.Count;
                ViewBag.ActiveCourses = courses.Count(c => c.IsActived);
                ViewBag.TotalOrders  = orders.Count;
                ViewBag.TotalRevenue = orders.Sum(o => o.Value);
            }

            return View();
        }

        // Placeholder stubs for Aluno sidebar links
        [HttpGet] public IActionResult MyCourses()      => View();
        [HttpGet] public IActionResult MyCertificates() => View();
        [HttpGet] public IActionResult MyOrders()       => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
