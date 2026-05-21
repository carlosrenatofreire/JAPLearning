using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Mvc.Models;
using System.Diagnostics;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly ITestimonialService _testimonialService;
        private readonly ICategoryService _categoryService;
        private readonly ITeamService _teamService;
        private readonly IArticleService _articleService;
        private readonly ITopicService _topicService;
        private readonly ILessonService _lessonService;
        private readonly ICourseRequirementService _requirementService;

        public HomeController(IUserService userService, ICourseService courseService,
            ITestimonialService testimonialService, ICategoryService categoryService,
            ITeamService teamService, IArticleService articleService, ITopicService topicService,
            ILessonService lessonService, ICourseRequirementService requirementService,
            INotificator notificator) : base(notificator)
        {
            _userService        = userService;
            _courseService      = courseService;
            _testimonialService = testimonialService;
            _categoryService    = categoryService;
            _teamService        = teamService;
            _articleService     = articleService;
            _topicService       = topicService;
            _lessonService      = lessonService;
            _requirementService = requirementService;
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

            ViewBag.TotalUsers    = users.Count;
            ViewBag.ActiveCourses = courses.Count(c => c.IsActived);

            return View();
        }

        // ── Páginas públicas ─────────────────────────────────────────────
        [AllowAnonymous] public IActionResult Privacy() => View();
        [AllowAnonymous] public IActionResult Faq()     => View();
        [AllowAnonymous] public IActionResult Terms()   => View();
        [AllowAnonymous] public IActionResult About()   => View();

        // ── Páginas públicas de catálogo ──────────────────────────────────
        [AllowAnonymous]
        public async Task<IActionResult> Courses(Guid? teamId = null)
        {
            var teams = await _teamService.GetAllAsync();
            ViewBag.Teams = teams.Where(t => t.IsActived).OrderBy(t => t.Name).ToList();

            if (teamId == null)
                return View(new List<Course>());

            var selectedTeam = teams.FirstOrDefault(t => t.Id == teamId);
            if (selectedTeam == null) return View(new List<Course>());

            var categories = await _categoryService.GetAllAsync();
            var teamCatIds = categories.Where(c => c.IsActived && c.TeamId == teamId)
                                       .Select(c => c.Id).ToHashSet();

            ViewBag.SelectedTeam = selectedTeam;
            ViewBag.Categories   = categories.Where(c => c.IsActived && c.TeamId == teamId)
                                             .OrderBy(c => c.Name).ToList();

            var courses = await _courseService.GetAllAsync();
            return View(courses.Where(c => c.IsActived && teamCatIds.Contains(c.CategoryId))
                               .OrderBy(c => c.Title).ToList());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Areas(Guid? teamId = null)
        {
            var teams = await _teamService.GetAllAsync();
            ViewBag.Teams = teams.Where(t => t.IsActived).OrderBy(t => t.Name).ToList();

            if (teamId == null)
                return View(new List<JAPLearning.Business.Models.Domains.Parameters.Category>());

            var selectedTeam = teams.FirstOrDefault(t => t.Id == teamId);
            if (selectedTeam == null)
                return View(new List<JAPLearning.Business.Models.Domains.Parameters.Category>());

            ViewBag.SelectedTeam = selectedTeam;

            var categories = await _categoryService.GetAllAsync();
            var courses    = await _courseService.GetAllAsync();
            var filtered   = categories.Where(c => c.IsActived && c.TeamId == teamId)
                                       .OrderBy(c => c.Name).ToList();

            ViewBag.CoursesCount = courses.Where(c => c.IsActived)
                                          .GroupBy(c => c.CategoryId)
                                          .ToDictionary(g => g.Key, g => g.Count());
            return View(filtered);
        }

        [AllowAnonymous]
        public async Task<IActionResult> CourseDetail(Guid id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null || !course.IsActived) return NotFound();

            var allTopics  = await _topicService.GetAllAsync();
            var allLessons = await _lessonService.GetAllAsync();
            var allCourses = await _courseService.GetAllAsync();

            var topics  = allTopics.Where(t => t.CourseId == id && t.IsActived).OrderBy(t => t.Order).ToList();
            var lessons = allLessons.Where(l => l.CourseId == id && l.IsActived).OrderBy(l => l.Order).ToList();

            var totalDuration = lessons
                .Where(l => l.TimeLesson.HasValue)
                .Aggregate(TimeSpan.Zero, (sum, l) => sum + l.TimeLesson!.Value);

            // Pré-requisitos com navegação enriquecida
            var requirements = await _requirementService.GetByCourseAsync(id);
            var coursesById  = allCourses.ToDictionary(c => c.Id);
            foreach (var req in requirements)
                if (coursesById.TryGetValue(req.PrerequisiteCourseId, out var prereq))
                    req.PrerequisiteCourse = prereq;

            // Duração por pré-requisito
            var prereqDurations = requirements.ToDictionary(
                r => r.PrerequisiteCourseId,
                r => allLessons
                    .Where(l => l.CourseId == r.PrerequisiteCourseId && l.IsActived && l.TimeLesson.HasValue)
                    .Aggregate(TimeSpan.Zero, (s, l) => s + l.TimeLesson!.Value));

            ViewBag.Topics          = topics;
            ViewBag.Lessons         = lessons;
            ViewBag.TotalDuration   = totalDuration;
            ViewBag.TotalLessons    = lessons.Count;
            ViewBag.FreePreview     = lessons.Where(l => l.IsFreePreview).ToList();
            ViewBag.Requirements    = requirements;
            ViewBag.PrereqDurations = prereqDurations;

            return View(course);
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
