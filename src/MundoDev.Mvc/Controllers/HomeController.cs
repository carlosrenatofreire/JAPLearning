using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Interfaces.Services.Parameters;
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
        private readonly ITestimonialService _testimonialService;
        private readonly IPlanService _planService;
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;

        public HomeController(IUserService userService, ICourseService courseService,
            IOrderService orderService, ITestimonialService testimonialService,
            IPlanService planService, ICategoryService categoryService,
            IArticleService articleService,
            INotificator notificator) : base(notificator)
        {
            _userService        = userService;
            _courseService      = courseService;
            _orderService       = orderService;
            _testimonialService = testimonialService;
            _planService        = planService;
            _categoryService    = categoryService;
            _articleService     = articleService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Utilizador não autenticado → landing page pública
            if (!User.Identity!.IsAuthenticated)
            {
                var testimonials = await _testimonialService.GetAllAsync();
                ViewBag.Testimonials = testimonials
                    .Where(t => t.IsActived && t.Featured)
                    .OrderByDescending(t => t.Rating)
                    .Take(6)
                    .Select(t => new {
                        t.AuthorName, t.Role, t.City, t.Quote,
                        t.PhotoUrl, t.LinkedinUrl, t.Rating
                    })
                    .ToList<dynamic>();
                return View("Landing");
            }

            // Alunos → área do aluno
            if (User.IsInRole("Aluno"))
                return RedirectToAction("Dashboard", "Student");

            // Admin / Supervisor → dashboard administrativo
            ViewData["ActiveMenu"] = "dashboard";

            var users   = await _userService.GetAllAsync();
            var courses = await _courseService.GetAllAsync();
            var orders  = await _orderService.GetAllAsync();

            ViewBag.TotalUsers    = users.Count;
            ViewBag.ActiveCourses = courses.Count(c => c.IsActived);
            ViewBag.TotalOrders   = orders.Count;
            ViewBag.TotalRevenue  = orders.Sum(o => o.Value);

            return View();
        }

        // ── Páginas públicas ─────────────────────────────────────────────
        [AllowAnonymous] public IActionResult Privacy() => View();
        [AllowAnonymous] public IActionResult Faq()     => View();
        [AllowAnonymous] public IActionResult Terms()   => View();
        [AllowAnonymous] public IActionResult About()   => View();

        // ── Páginas públicas de catálogo ──────────────────────────────────
        [AllowAnonymous]
        public async Task<IActionResult> Courses()
        {
            var courses    = await _courseService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Where(c => c.IsActived).OrderBy(c => c.Name).ToList();
            return View(courses.Where(c => c.IsActived).OrderBy(c => c.Title).ToList());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Areas()
        {
            var categories = await _categoryService.GetAllAsync();
            var courses    = await _courseService.GetAllAsync();
            ViewBag.CoursesCount = courses.Where(c => c.IsActived)
                                          .GroupBy(c => c.CategoryId)
                                          .ToDictionary(g => g.Key, g => g.Count());
            return View(categories.Where(c => c.IsActived).OrderBy(c => c.Name).ToList());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Plans()
        {
            var plans = await _planService.GetAllAsync();
            return View(plans.Where(p => p.IsActived).OrderBy(p => p.Price).ToList());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Blog()
        {
            var articles = await _articleService.GetAllAsync();
            return View(articles.Where(a => a.IsActived).OrderByDescending(a => a.PublishDate).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
