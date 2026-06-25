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
using System.Security.Claims;

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
        private readonly ICertificateService _certificateService;

        public HomeController(IUserService userService, ICourseService courseService,
            ITestimonialService testimonialService, ICategoryService categoryService,
            ITeamService teamService, IArticleService articleService, ITopicService topicService,
            ILessonService lessonService, ICourseRequirementService requirementService,
            IUserCourseLessonService userCourseLessonService,
            ICertificateService certificateService,
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
            _certificateService      = certificateService;
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

            var users      = await _userService.GetAllAsync();
            var courses    = await _courseService.GetAllAsync();
            var teams      = await _teamService.GetAllAsync();
            var lessons    = await _lessonService.GetAllAsync();
            var progress   = await _userCourseLessonService.GetAllAsync();
            var certs      = await _certificateService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            // ── Branch Supervisor (scoped ao departamento) ───────────────
            if (User.IsInRole("Supervisor"))
            {
                var teamIdClaim = User.FindFirstValue("TeamId");
                var teamId      = Guid.TryParse(teamIdClaim, out var tid) ? tid : Guid.Empty;
                var team        = teams.FirstOrDefault(t => t.Id == teamId);

                // IDs de categorias e cursos do departamento
                var deptCatIds    = categories.Where(c => c.TeamId == teamId).Select(c => c.Id).ToHashSet();
                var deptCourses   = courses.Where(c => deptCatIds.Contains(c.CategoryId)).ToList();
                var deptCourseIds = deptCourses.Select(c => c.Id).ToHashSet();
                var deptLessons   = lessons.Where(l => deptCourseIds.Contains(l.CourseId)).ToList();
                var deptLessonIds = deptLessons.Select(l => l.Id).ToHashSet();

                // Alunos da equipa
                var deptUsers   = users.Where(u => u.TeamId == teamId).ToList();
                var deptUserIds = deptUsers.Select(u => u.Id).ToHashSet();

                // Progresso filtrado ao departamento
                var deptProgress = progress.Where(p => deptLessonIds.Contains(p.LessonId)).ToList();

                // Certificados do departamento (formações do dept)
                var deptCerts = certs.Where(c => deptCourseIds.Contains(c.CourseId)).ToList();

                ViewBag.TeamName          = team?.Name ?? "—";
                ViewBag.TotalUsers        = deptUsers.Count(u => u.Role?.Name == "Aluno");
                ViewBag.ActiveCourses     = deptCourses.Count(c => c.IsActived && !c.IsBrief);
                ViewBag.BriefCourses      = deptCourses.Count(c => c.IsActived && c.IsBrief);
                ViewBag.TotalCategories   = deptCatIds.Count;
                ViewBag.TotalCertificates = deptCerts.Count;

                // Top 7 Alunos do departamento
                var usersById   = deptUsers.ToDictionary(u => u.Id);
                var topStudents = deptProgress
                    .Where(p => p.CompletedDate.HasValue && deptUserIds.Contains(p.UserId))
                    .GroupBy(p => p.UserId)
                    .Select(g => new { UserId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(7)
                    .Select(x => {
                        usersById.TryGetValue(x.UserId, out var usr);
                        return new { Id = x.UserId, Name = usr != null ? $"{usr.FirstName} {usr.LastName}".Trim() : "—", Photo = usr?.PhotoUrl, Count = x.Count };
                    })
                    .ToList<dynamic>();
                ViewBag.TopStudents = topStudents;

                // Top 7 Lições mais concluídas do departamento
                var lessonsById = deptLessons.ToDictionary(l => l.Id);
                var topLessons  = deptProgress
                    .Where(p => p.CompletedDate.HasValue && lessonsById.ContainsKey(p.LessonId))
                    .GroupBy(p => p.LessonId)
                    .Select(g => new { LessonId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(7)
                    .Select(x => new {
                        Name  = lessonsById.TryGetValue(x.LessonId, out var l) ? l.Name : "—",
                        Count = x.Count
                    })
                    .ToList<dynamic>();
                ViewBag.TopLessons = topLessons;

                // Top 7 Formações do departamento
                var topCoursesSup = deptProgress
                    .Where(p => p.CompletedDate.HasValue && lessonsById.ContainsKey(p.LessonId))
                    .GroupBy(p => lessonsById[p.LessonId].CourseId)
                    .Select(g => new { CourseId = g.Key, Students = g.Select(x => x.UserId).Distinct().Count() })
                    .OrderByDescending(x => x.Students)
                    .Take(7)
                    .Select(x => new {
                        Title    = deptCourses.FirstOrDefault(c => c.Id == x.CourseId)?.Title ?? "—",
                        Students = x.Students
                    })
                    .ToList<dynamic>();
                ViewBag.TopCourses = topCoursesSup;

                return View("_DashboardSupervisor");
            }

            // ── Branch Admin (global) ────────────────────────────────────
            ViewBag.TotalUsers        = users.Count(u => u.Role?.Name == "Aluno");
            ViewBag.ActiveCourses     = courses.Count(c => c.IsActived && !c.IsBrief);
            ViewBag.BriefCourses      = courses.Count(c => c.IsActived && c.IsBrief);
            ViewBag.TotalTeams        = teams.Count(t => t.IsActived);
            ViewBag.TotalCertificates = certs.Count;

            // ── Top 7 Alunos por aulas concluídas ───────────────────────
            var usersById2   = users.ToDictionary(usr => usr.Id);
            var teamsById    = teams.ToDictionary(tm  => tm.Id);
            var topStudents2 = progress
                .Where(p => p.CompletedDate.HasValue)
                .GroupBy(p => p.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(7)
                .Select(x => {
                    usersById2.TryGetValue(x.UserId, out var usr);
                    return new { Id = x.UserId, Name = usr != null ? $"{usr.FirstName} {usr.LastName}".Trim() : "—", Photo = usr?.PhotoUrl, Count = x.Count };
                })
                .ToList<dynamic>();
            ViewBag.TopStudents = topStudents2;

            // ── Top 7 Equipas por aulas concluídas dos seus membros ─────
            var topTeams = progress
                .Where(p => p.CompletedDate.HasValue)
                .GroupBy(p => p.UserId)
                .Select(g => {
                    usersById2.TryGetValue(g.Key, out var usr);
                    return new { TeamId = usr?.TeamId, Count = g.Count() };
                })
                .Where(x => x.TeamId.HasValue)
                .GroupBy(x => x.TeamId!.Value)
                .Select(g => new { TeamId = g.Key, Count = g.Sum(x => x.Count) })
                .OrderByDescending(x => x.Count)
                .Take(7)
                .Select(x => {
                    teamsById.TryGetValue(x.TeamId, out var tm);
                    return new { Name = tm?.Name ?? "—", Count = x.Count };
                })
                .ToList<dynamic>();
            ViewBag.TopTeams = topTeams;

            // ── Top 7 Formações mais populares ──────────────────────────
            var lessonsById2 = lessons.ToDictionary(l => l.Id);
            var topCourses   = progress
                .Where(p => p.CompletedDate.HasValue && lessonsById2.ContainsKey(p.LessonId))
                .GroupBy(p => lessonsById2[p.LessonId].CourseId)
                .Select(g => new { CourseId = g.Key, Students = g.Select(x => x.UserId).Distinct().Count() })
                .OrderByDescending(x => x.Students)
                .Take(7)
                .Select(x => new {
                    Title    = courses.FirstOrDefault(c => c.Id == x.CourseId)?.Title ?? "—",
                    Students = x.Students
                })
                .ToList<dynamic>();
            ViewBag.TopCourses = topCourses;

            return View();
        }

        // ── Páginas públicas ─────────────────────────────────────────────
        [AllowAnonymous] public IActionResult Privacy()   => View();
        [AllowAnonymous] public IActionResult Faq()       => View();
        [AllowAnonymous] public IActionResult Terms()     => View();
        [AllowAnonymous] public IActionResult About()     => View();
        [AllowAnonymous] public IActionResult PorqueFormacao() => View();

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
