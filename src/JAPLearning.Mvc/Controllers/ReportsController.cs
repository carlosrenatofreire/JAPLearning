using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Interfaces.Services.Relationships;
using System.Security.Claims;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class ReportsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly ITeamService _teamService;
        private readonly ILessonService _lessonService;
        private readonly IUserCourseLessonService _progressService;
        private readonly ICertificateService _certificateService;

        public ReportsController(
            IUserService userService,
            ICourseService courseService,
            ITeamService teamService,
            ILessonService lessonService,
            IUserCourseLessonService progressService,
            ICertificateService certificateService,
            INotificator notificator) : base(notificator)
        {
            _userService         = userService;
            _courseService       = courseService;
            _teamService         = teamService;
            _lessonService       = lessonService;
            _progressService     = progressService;
            _certificateService  = certificateService;
        }

        // ── Auxiliar: TeamId do Supervisor (null = Admin) ────────────────
        private Guid? GetSupervisorTeamId()
        {
            if (!User.IsInRole("Supervisor")) return null;
            var claim = User.FindFirstValue("TeamId");
            return Guid.TryParse(claim, out var tid) ? tid : (Guid?)null;
        }

        // ── Ranking de Alunos ────────────────────────────────────────────
        public async Task<IActionResult> Students()
        {
            ViewData["ActiveMenu"] = "dashboard";

            var supervisorTeamId = GetSupervisorTeamId();

            var allUsers = await _userService.GetAllAsync();
            var teams    = await _teamService.GetAllAsync();
            var lessons  = await _lessonService.GetAllAsync();
            var progress = await _progressService.GetAllAsync();
            var certs    = await _certificateService.GetAllAsync();

            // Filtrar utilizadores pelo departamento do Supervisor
            var users = supervisorTeamId.HasValue
                ? allUsers.Where(u => u.TeamId == supervisorTeamId.Value).ToList()
                : allUsers;

            var teamsById  = teams.ToDictionary(t => t.Id);
            var lessonsById = lessons.ToDictionary(l => l.Id);

            var completedByUser = progress
                .Where(p => p.CompletedDate.HasValue)
                .GroupBy(p => p.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            var certsByUser = certs
                .GroupBy(c => c.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            var secondsByUser = progress
                .Where(p => p.CompletedDate.HasValue && lessonsById.ContainsKey(p.LessonId))
                .GroupBy(p => p.UserId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => lessonsById.TryGetValue(x.LessonId, out var l) && l.TimeLesson.HasValue
                                    ? (int)l.TimeLesson!.Value.TotalSeconds
                                    : 0));

            var students = users
                .Where(u => u.IsActived && !string.IsNullOrEmpty(u.FirstName))
                .Select(u =>
                {
                    teamsById.TryGetValue(u.TeamId, out var team);
                    completedByUser.TryGetValue(u.Id, out var lessonCount);
                    certsByUser.TryGetValue(u.Id, out var certsCount);
                    secondsByUser.TryGetValue(u.Id, out var totalSecs);
                    var hours      = totalSecs / 3600;
                    var minutes    = (totalSecs % 3600) / 60;
                    var hoursLabel = hours > 0
                        ? (minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h")
                        : (minutes > 0 ? $"{minutes}m" : "—");
                    return new
                    {
                        u.Id,
                        Name       = $"{u.FirstName} {u.LastName}".Trim(),
                        u.Email,
                        Photo      = u.PhotoUrl,
                        TeamName   = team?.Name ?? "—",
                        Lessons    = lessonCount,
                        Certs      = certsCount,
                        TotalSecs  = totalSecs,
                        HoursLabel = hoursLabel
                    };
                })
                .OrderByDescending(x => x.Lessons)
                .ThenBy(x => x.Name)
                .ToList<dynamic>();

            ViewBag.Students = students;
            return View();
        }

        // ── Detalhe de Aluno ─────────────────────────────────────────────
        public async Task<IActionResult> StudentDetail(Guid id)
        {
            ViewData["ActiveMenu"] = "dashboard";

            var supervisorTeamId = GetSupervisorTeamId();

            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Supervisor só pode ver alunos da sua equipa
            if (supervisorTeamId.HasValue && user.TeamId != supervisorTeamId.Value)
                return Forbid();

            var allLessons  = await _lessonService.GetAllAsync();
            var allCourses  = await _courseService.GetAllAsync();
            var allCerts    = await _certificateService.GetAllAsync();
            var userProgress = await _progressService.GetByUserAsync(id);

            var lessonsById  = allLessons.ToDictionary(l => l.Id);
            var coursesById  = allCourses.ToDictionary(c => c.Id);
            var userCerts    = allCerts.Where(c => c.UserId == id).ToList();

            // ── KPIs ─────────────────────────────────────────────────────
            var completedProgress = userProgress.Where(p => p.CompletedDate.HasValue).ToList();
            var totalLessons      = completedProgress.Count;
            var totalSecs         = completedProgress
                .Sum(p => lessonsById.TryGetValue(p.LessonId, out var l) && l.TimeLesson.HasValue
                          ? (int)l.TimeLesson!.Value.TotalSeconds : 0);
            var hours      = totalSecs / 3600;
            var minutes    = (totalSecs % 3600) / 60;
            var hoursLabel = hours > 0
                ? (minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h")
                : (minutes > 0 ? $"{minutes}m" : "—");

            // ── Progresso por Formação ────────────────────────────────────
            var lessonsByCourse = allLessons
                .Where(l => l.IsActived)
                .GroupBy(l => l.CourseId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var completedLessonIdsByUser = completedProgress
                .Select(p => p.LessonId)
                .ToHashSet();

            var touchedCourseIds = userProgress
                .Where(p => lessonsById.ContainsKey(p.LessonId))
                .Select(p => lessonsById[p.LessonId].CourseId)
                .Distinct()
                .ToList();

            var courseRows = touchedCourseIds
                .Where(cid => coursesById.ContainsKey(cid))
                .Select(cid =>
                {
                    var course        = coursesById[cid];
                    var totalInCourse = lessonsByCourse.TryGetValue(cid, out var ls) ? ls.Count : 0;
                    var doneInCourse  = lessonsByCourse.TryGetValue(cid, out var ls2)
                        ? ls2.Count(l => completedLessonIdsByUser.Contains(l.Id))
                        : 0;
                    var pct  = totalInCourse > 0 ? (int)Math.Round((double)doneInCourse / totalInCourse * 100) : 0;
                    var cert = userCerts.FirstOrDefault(c => c.CourseId == cid);
                    return new
                    {
                        CourseId    = cid,
                        Title       = course.Title,
                        Thumbnail   = course.Thumbnail,
                        Done        = doneInCourse,
                        Total       = totalInCourse,
                        Pct         = pct,
                        HasCert     = cert != null,
                        ScorePct    = cert?.ScorePercent ?? 0,
                        CertDate    = cert?.CompletedDate,
                        CertId      = cert?.Id
                    };
                })
                .OrderByDescending(x => x.Pct)
                .ThenBy(x => x.Title)
                .ToList<dynamic>();

            var team = (await _teamService.GetAllAsync()).FirstOrDefault(t => t.Id == user.TeamId);

            // ── Tópicos com lições aninhadas por Formação ────────────────
            var lessonDetails = allLessons
                .Where(l => l.IsActived)
                .GroupBy(l => l.CourseId)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .GroupBy(l => new
                        {
                            TopicId    = l.TopicId,
                            TopicName  = l.Topic?.Name ?? "—",
                            TopicOrder = l.Topic?.Order ?? 0
                        })
                        .OrderBy(tg => tg.Key.TopicOrder)
                        .Select(tg => new
                        {
                            TopicName = tg.Key.TopicName,
                            Lessons   = tg.OrderBy(l => l.Order).Select(l => new
                            {
                                l.Id,
                                l.Name,
                                l.Order,
                                Duration = l.TimeLesson.HasValue
                                    ? (l.TimeLesson.Value.TotalHours >= 1
                                        ? l.TimeLesson.Value.ToString(@"h\:mm\:ss")
                                        : l.TimeLesson.Value.ToString(@"mm\:ss"))
                                    : (string?)null,
                                IsDone   = completedLessonIdsByUser.Contains(l.Id),
                                DoneDate = userProgress
                                    .Where(p => p.LessonId == l.Id && p.CompletedDate.HasValue)
                                    .Select(p => p.CompletedDate)
                                    .FirstOrDefault()
                            }).ToList<dynamic>()
                        }).ToList<dynamic>());

            ViewBag.User            = user;
            ViewBag.TeamName        = team?.Name ?? "—";
            ViewBag.TotalLessons    = totalLessons;
            ViewBag.HoursLabel      = hoursLabel;
            ViewBag.TotalCerts      = userCerts.Count;
            ViewBag.CoursesCount    = touchedCourseIds.Count;
            ViewBag.CourseRows      = courseRows;
            ViewBag.LessonsByCourse = lessonDetails;
            ViewBag.Certs           = userCerts
                .Where(c => coursesById.ContainsKey(c.CourseId))
                .Select(c => new
                {
                    c.Id,
                    CourseTitle = coursesById[c.CourseId].Title,
                    c.ScorePercent,
                    c.CompletedDate,
                    c.ValidationCode
                })
                .OrderByDescending(c => c.CompletedDate)
                .ToList<dynamic>();

            return View();
        }

        // ── Ranking de Equipas ───────────────────────────────────────────
        public async Task<IActionResult> Teams()
        {
            ViewData["ActiveMenu"] = "dashboard";

            var supervisorTeamId = GetSupervisorTeamId();

            var users    = await _userService.GetAllAsync();
            var allTeams = await _teamService.GetAllAsync();
            var progress = await _progressService.GetAllAsync();
            var certs    = await _certificateService.GetAllAsync();

            // Filtrar equipas: Supervisor vê só a sua
            var teams = supervisorTeamId.HasValue
                ? allTeams.Where(t => t.Id == supervisorTeamId.Value).ToList()
                : allTeams.Where(t => t.IsActived).ToList();

            var completedByUser = progress
                .Where(p => p.CompletedDate.HasValue)
                .GroupBy(p => p.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            var certsByUser = certs
                .GroupBy(c => c.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            var teamRows = teams
                .Select(t =>
                {
                    var members   = users.Where(u => u.TeamId == t.Id && u.IsActived).ToList();
                    var totalLess = members.Sum(u => completedByUser.GetValueOrDefault(u.Id, 0));
                    var totalCert = members.Sum(u => certsByUser.GetValueOrDefault(u.Id, 0));
                    var avg       = members.Count > 0 ? (double)totalLess / members.Count : 0;
                    return new
                    {
                        t.Id,
                        t.Name,
                        Members = members.Count,
                        Lessons = totalLess,
                        Average = Math.Round(avg, 1),
                        Certs   = totalCert
                    };
                })
                .OrderByDescending(x => x.Lessons)
                .ThenBy(x => x.Name)
                .ToList<dynamic>();

            ViewBag.Teams = teamRows;
            return View();
        }

        // ── Ranking de Formações ─────────────────────────────────────────
        public async Task<IActionResult> Courses()
        {
            ViewData["ActiveMenu"] = "dashboard";

            var supervisorTeamId = GetSupervisorTeamId();

            var allCourses = await _courseService.GetAllAsync();
            var lessons    = await _lessonService.GetAllAsync();
            var progress   = await _progressService.GetAllAsync();
            var certs      = await _certificateService.GetAllAsync();

            // Filtrar formações pelo departamento do Supervisor
            var courses = supervisorTeamId.HasValue
                ? allCourses.Where(c => c.IsActived && c.Category?.TeamId == supervisorTeamId.Value).ToList()
                : allCourses.Where(c => c.IsActived).ToList();

            var lessonsById = lessons.ToDictionary(l => l.Id);

            var completedByCourse = progress
                .Where(p => p.CompletedDate.HasValue && lessonsById.ContainsKey(p.LessonId))
                .GroupBy(p => lessonsById[p.LessonId].CourseId)
                .ToDictionary(
                    g => g.Key,
                    g => new { Students = g.Select(x => x.UserId).Distinct().Count(), LessonsTotal = g.Count() });

            var certsByCourse = certs
                .GroupBy(c => c.CourseId)
                .ToDictionary(g => g.Key, g => g.Count());

            var lessonCountByCourse = lessons
                .Where(l => l.IsActived)
                .GroupBy(l => l.CourseId)
                .ToDictionary(g => g.Key, g => g.Count());

            var courseRows = courses
                .Select(c =>
                {
                    completedByCourse.TryGetValue(c.Id, out var stats);
                    certsByCourse.TryGetValue(c.Id, out var certsCount);
                    lessonCountByCourse.TryGetValue(c.Id, out var totalLess);
                    return new
                    {
                        c.Id,
                        c.Title,
                        Category  = c.Category?.Name ?? "—",
                        Level     = c.Level?.Name ?? "—",
                        Students  = stats?.Students ?? 0,
                        Lessons   = stats?.LessonsTotal ?? 0,
                        TotalLess = totalLess,
                        Certs     = certsCount
                    };
                })
                .OrderByDescending(x => x.Students)
                .ThenBy(x => x.Title)
                .ToList<dynamic>();

            ViewBag.Courses = courseRows;
            return View();
        }
    }
}
