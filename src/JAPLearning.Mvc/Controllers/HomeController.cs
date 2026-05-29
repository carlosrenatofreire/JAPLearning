using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Domains.Relationships;
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
        private readonly IUserCourseLessonService _userCourseLessonService;

        public HomeController(IUserService userService, ICourseService courseService,
            ITestimonialService testimonialService, ICategoryService categoryService,
            ITeamService teamService, IArticleService articleService, ITopicService topicService,
            ILessonService lessonService, ICourseRequirementService requirementService,
            IUserCourseLessonService userCourseLessonService,
            INotificator notificator) : base(notificator)
        {
            _userService             = userService;
            _courseService           = courseService;
            _testimonialService      = testimonialService;
            _categoryService         = categoryService;
            _teamService             = teamService;
            _articleService          = articleService;
            _topicService            = topicService;
            _lessonService           = lessonService;
            _requirementService      = requirementService;
            _userCourseLessonService = userCourseLessonService;
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
                    .OrderBy(t => t.DisplayOrder)
                    .ThenByDescending(t => t.Rating)
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

            var users    = await _userService.GetAllAsync();
            var courses  = await _courseService.GetAllAsync();
            var teams    = await _teamService.GetAllAsync();
            var lessons  = await _lessonService.GetAllAsync();
            var progress = await _userCourseLessonService.GetAllAsync();

            ViewBag.TotalUsers        = users.Count;
            ViewBag.ActiveCourses     = courses.Count(c => c.IsActived);
            ViewBag.TotalTeams        = teams.Count(t => t.IsActived);
            ViewBag.TotalCertificates = users.Sum(u => u.Certificates?.Count ?? 0);

            // ── Top 10 Alunos por aulas concluídas ──────────────────────
            var usersById   = users.ToDictionary(usr => usr.Id);
            var teamsById   = teams.ToDictionary(tm  => tm.Id);
            var topStudents = progress
                .Where(p => p.CompletedDate.HasValue)
                .GroupBy(p => p.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .Select(x => {
                    usersById.TryGetValue(x.UserId, out var usr);
                    return new { Name = usr != null ? $"{usr.FirstName} {usr.LastName}".Trim() : "—", Photo = usr?.PhotoUrl, Count = x.Count };
                })
                .ToList<dynamic>();
            ViewBag.TopStudents = topStudents;

            // ── Top 10 Equipas por aulas concluídas dos seus membros ────
            var topTeams = progress
                .Where(p => p.CompletedDate.HasValue)
                .GroupBy(p => p.UserId)
                .Select(g => {
                    usersById.TryGetValue(g.Key, out var usr);
                    return new { TeamId = usr?.TeamId, Count = g.Count() };
                })
                .Where(x => x.TeamId.HasValue)
                .GroupBy(x => x.TeamId!.Value)
                .Select(g => new { TeamId = g.Key, Count = g.Sum(x => x.Count) })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .Select(x => {
                    teamsById.TryGetValue(x.TeamId, out var tm);
                    return new { Name = tm?.Name ?? "—", Count = x.Count };
                })
                .ToList<dynamic>();
            ViewBag.TopTeams = topTeams;

            // ── Top 5 Cursos mais populares (alunos únicos com progresso) ─
            var lessonsById = lessons.ToDictionary(l => l.Id);
            var topCourses  = progress
                .Where(p => p.CompletedDate.HasValue && lessonsById.ContainsKey(p.LessonId))
                .GroupBy(p => lessonsById[p.LessonId].CourseId)
                .Select(g => new { CourseId = g.Key, Students = g.Select(x => x.UserId).Distinct().Count() })
                .OrderByDescending(x => x.Students)
                .Take(5)
                .Select(x => new {
                    Title    = courses.FirstOrDefault(c => c.Id == x.CourseId)?.Title ?? "—",
                    Students = x.Students
                })
                .ToList<dynamic>();
            ViewBag.TopCourses = topCourses;

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
            var teams      = await _teamService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var courses    = await _courseService.GetAllAsync();

            // Apenas equipas com pelo menos uma categoria activa que tenha cursos activos
            var teamIdsWithContent = categories
                .Where(c => c.IsActived && courses.Any(co => co.IsActived && co.CategoryId == c.Id))
                .Select(c => c.TeamId)
                .ToHashSet();

            ViewBag.Teams = teams
                .Where(t => t.IsActived && teamIdsWithContent.Contains(t.Id))
                .OrderBy(t => t.Name).ToList();

            // Contagem de cursos activos por equipa
            var catByTeam = categories
                .Where(c => c.IsActived)
                .GroupBy(c => c.TeamId)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Id).ToHashSet());

            ViewBag.TeamCourseCounts = catByTeam.ToDictionary(
                kv => kv.Key,
                kv => courses.Count(co => co.IsActived && kv.Value.Contains(co.CategoryId)));

            ViewBag.TeamCategoryCounts = catByTeam.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.Count);

            if (teamId == null)
                return View(new List<Course>());

            var selectedTeam = teams.FirstOrDefault(t => t.Id == teamId);
            if (selectedTeam == null) return View(new List<Course>());

            var teamCatIds = categories.Where(c => c.IsActived && c.TeamId == teamId)
                                       .Select(c => c.Id).ToHashSet();

            ViewBag.SelectedTeam = selectedTeam;
            ViewBag.Categories   = categories.Where(c => c.IsActived && c.TeamId == teamId)
                                             .OrderBy(c => c.Name).ToList();

            // Duração total por formação (soma das lições)
            var allLessons = await _lessonService.GetAllAsync();
            ViewBag.CourseDurations = allLessons
                .Where(l => l.IsActived && l.TimeLesson.HasValue)
                .GroupBy(l => l.CourseId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Aggregate(TimeSpan.Zero, (sum, l) => sum + l.TimeLesson!.Value));

            return View(courses.Where(c => c.IsActived && teamCatIds.Contains(c.CategoryId))
                               .OrderBy(c => c.Title).ToList());
        }

        [AllowAnonymous]
        public IActionResult Areas() => RedirectToAction("Courses");

        [AllowAnonymous]
        public async Task<IActionResult> CourseDetail(Guid id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null || !course.IsActived) return NotFound();
            if (course.IsBrief) return RedirectToAction("Courses", new { teamId = course.Category?.TeamId });

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
            var active   = articles.Where(a => a.IsActived).OrderByDescending(a => a.PublishDate).ToList();
            ViewBag.Subjects = active
                .Where(a => a.Subject != null)
                .Select(a => a.Subject!)
                .DistinctBy(s => s.Id)
                .OrderBy(s => s.Name)
                .ToList();
            return View(active);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ArticleDetail(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug)) return NotFound();
            var article = await _articleService.GetBySlugAsync(slug);
            if (article == null || !article.IsActived) return NotFound();
            return View(article);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
