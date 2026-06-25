using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Mvc.ViewModels.Entities;
using System.Security.Claims;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class LessonsController : BaseController
    {
        private readonly ILessonService _service;
        private readonly ICourseService _courseService;
        private readonly ITopicService _topicService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public LessonsController(ILessonService service, ICourseService courseService,
            ITopicService topicService, ITeamService teamService,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _courseService = courseService;
            _topicService = topicService;
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "lessons";
            var allCourses = await _courseService.GetAllAsync();
            var allTopics  = await _topicService.GetAllAsync();
            var teams      = await _teamService.GetAllAsync();

            // ── Filtrar por departamento para Supervisor ─────────────────
            var isSupervisor     = User.IsInRole("Supervisor");
            var teamIdClaim      = User.FindFirstValue("TeamId");
            var supervisorTeamId = isSupervisor && Guid.TryParse(teamIdClaim, out var tid) ? tid : (Guid?)null;

            var visibleCourseIds = supervisorTeamId.HasValue
                ? allCourses.Where(c => c.Category?.TeamId == supervisorTeamId.Value).Select(c => c.Id).ToHashSet()
                : null;

            var allLessons = await _service.GetAllAsync();
            var list       = supervisorTeamId.HasValue
                ? allLessons.Where(l => visibleCourseIds!.Contains(l.CourseId)).ToList()
                : allLessons;

            var courses = supervisorTeamId.HasValue
                ? allCourses.Where(c => c.IsActived && c.Category?.TeamId == supervisorTeamId.Value).ToList()
                : allCourses.Where(c => c.IsActived && c.Category != null).ToList();

            var topics = supervisorTeamId.HasValue
                ? allTopics.Where(t => t.IsActived && visibleCourseIds!.Contains(t.CourseId)).ToList()
                : allTopics.Where(t => t.IsActived).ToList();

            var visibleTeams = supervisorTeamId.HasValue
                ? teams.Where(t => t.Id == supervisorTeamId.Value).ToList()
                : teams.Where(t => t.IsActived).ToList();

            ViewBag.SupervisorTeamId = supervisorTeamId?.ToString() ?? "";
            ViewBag.FilterTeams = visibleTeams
                .OrderBy(t => t.Name)
                .Select(t => new { id = t.Id, name = t.Name })
                .ToList();

            ViewBag.FilterCourses = courses
                .OrderBy(c => c.Title)
                .Select(c => new
                {
                    id     = c.Id,
                    name   = c.Title,
                    label  = $"{c.Title} ({c.Category!.Name})",
                    teamId = c.Category!.TeamId
                })
                .ToList();

            ViewBag.FilterTopics = topics
                .OrderBy(t => t.CourseId)
                .ThenBy(t => t.Order)
                .Select(t => new { id = t.Id, name = t.Name, courseId = t.CourseId })
                .ToList();

            return View(_mapper.Map<List<LessonViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "lessons";
            await PopulateDropdownsAsync();
            return View(new LessonViewModel { IsActived = true, Order = 1, TimeLesson = TimeSpan.FromMinutes(5) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonViewModel vm)
        {
            ViewData["ActiveMenu"] = "lessons";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(); return View(vm); }
            var entity = _mapper.Map<Lesson>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(); return View(vm); }
            TempData["Success"] = "Lição criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "lessons";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateDropdownsAsync(entity.CourseId);
            return View(_mapper.Map<LessonViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LessonViewModel vm)
        {
            ViewData["ActiveMenu"] = "lessons";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(vm.CourseId); return View(vm); }
            var entity = _mapper.Map<Lesson>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(vm.CourseId); return View(vm); }
            TempData["Success"] = "Lição actualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Supervisor")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supervisorTeamId = GetSupervisorTeamId();
            if (supervisorTeamId.HasValue)
            {
                var lesson = await _service.GetByIdAsync(id);
                if (lesson == null) return NotFound();
                var courses = await _courseService.GetAllAsync();
                var course  = courses.FirstOrDefault(c => c.Id == lesson.CourseId);
                if (course?.Category?.TeamId != supervisorTeamId.Value)
                    return Forbid();
            }

            if (!await _service.DeleteAsync(id))
            {
                AddWarnings();
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Lição eliminada.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX — courses filtered by team
        [HttpGet]
        public async Task<IActionResult> GetCoursesByTeam(Guid teamId)
        {
            var all = await _courseService.GetAllAsync();
            var filtered = all
                .Where(c => c.IsActived && c.Category != null && c.Category.TeamId == teamId)
                .OrderBy(c => c.Title)
                .Select(c => new { value = c.Id, text = c.Title });
            return Json(filtered);
        }

        private Guid? GetSupervisorTeamId()
        {
            if (!User.IsInRole("Supervisor")) return null;
            return Guid.TryParse(User.FindFirstValue("TeamId"), out var tid) ? tid : (Guid?)null;
        }

        private async Task PopulateDropdownsAsync(Guid? selectedCourseId = null)
        {
            var supervisorTeamId = GetSupervisorTeamId();

            var allCourses = await _courseService.GetAllAsync();
            var courses    = supervisorTeamId.HasValue
                ? allCourses.Where(c => c.IsActived && c.Category?.TeamId == supervisorTeamId.Value)
                : allCourses.Where(c => c.IsActived);
            ViewBag.Courses = new SelectList(courses, "Id", "Title", selectedCourseId);

            var topics = selectedCourseId.HasValue
                ? await _topicService.GetByCourseAsync(selectedCourseId.Value)
                : new List<Topic>();
            ViewBag.Topics = new SelectList(topics, "Id", "Name");

            var allTeams = await _teamService.GetAllAsync();
            var teams    = supervisorTeamId.HasValue
                ? allTeams.Where(t => t.Id == supervisorTeamId.Value)
                : allTeams.Where(t => t.IsActived);
            ViewBag.Teams            = new SelectList(teams, "Id", "Name");
            ViewBag.SupervisorTeamId = supervisorTeamId?.ToString() ?? "";

            if (selectedCourseId.HasValue)
            {
                var course = allCourses.FirstOrDefault(c => c.Id == selectedCourseId.Value);
                ViewBag.SelectedTeamId = course?.Category?.TeamId;
            }
        }
    }
}
