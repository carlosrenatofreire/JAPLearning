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
    public class TopicsController : BaseController
    {
        private readonly ITopicService _service;
        private readonly ICourseService _courseService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public TopicsController(ITopicService service, ICourseService courseService,
            ITeamService teamService, IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _courseService = courseService;
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "topics";
            var allCourses = await _courseService.GetAllAsync();
            var teams      = await _teamService.GetAllAsync();

            // ── Filtrar por departamento para Supervisor ─────────────────
            var isSupervisor     = User.IsInRole("Supervisor");
            var teamIdClaim      = User.FindFirstValue("TeamId");
            var supervisorTeamId = isSupervisor && Guid.TryParse(teamIdClaim, out var tid) ? tid : (Guid?)null;

            var visibleCourseIds = supervisorTeamId.HasValue
                ? allCourses.Where(c => c.Category?.TeamId == supervisorTeamId.Value).Select(c => c.Id).ToHashSet()
                : null;

            var allTopics = await _service.GetAllAsync();
            var list      = supervisorTeamId.HasValue
                ? allTopics.Where(t => visibleCourseIds!.Contains(t.CourseId)).ToList()
                : allTopics;

            var courses = supervisorTeamId.HasValue
                ? allCourses.Where(c => c.IsActived && c.Category?.TeamId == supervisorTeamId.Value).ToList()
                : allCourses.Where(c => c.IsActived && c.Category != null).ToList();

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

            return View(_mapper.Map<List<TopicViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "topics";
            await PopulateCoursesAsync();
            return View(new TopicViewModel { IsActived = true, Order = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TopicViewModel vm)
        {
            ViewData["ActiveMenu"] = "topics";
            if (!ModelState.IsValid) { await PopulateCoursesAsync(vm.CourseId); return View(vm); }
            var entity = _mapper.Map<Topic>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (!await _service.AddAsync(entity)) { AddWarnings(); await PopulateCoursesAsync(vm.CourseId); return View(vm); }
            TempData["Success"] = "Tópico criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "topics";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateCoursesAsync(entity.CourseId);
            return View(_mapper.Map<TopicViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TopicViewModel vm)
        {
            ViewData["ActiveMenu"] = "topics";
            if (!ModelState.IsValid) { await PopulateCoursesAsync(vm.CourseId); return View(vm); }
            var entity = _mapper.Map<Topic>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (!await _service.UpdateAsync(entity)) { AddWarnings(); await PopulateCoursesAsync(vm.CourseId); return View(vm); }
            TempData["Success"] = "Tópico actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Tópico eliminado.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX — topics filtered by course
        [HttpGet]
        public async Task<IActionResult> GetByCourse(Guid courseId)
        {
            var topics = await _service.GetByCourseAsync(courseId);
            return Json(topics.Select(t => new { t.Id, t.Name }));
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

        private async Task PopulateCoursesAsync(Guid? selectedCourseId = null)
        {
            var supervisorTeamId = GetSupervisorTeamId();

            var allCourses = await _courseService.GetAllAsync();
            var courses    = supervisorTeamId.HasValue
                ? allCourses.Where(c => c.IsActived && c.Category?.TeamId == supervisorTeamId.Value)
                : allCourses.Where(c => c.IsActived);
            ViewBag.Courses = new SelectList(courses, "Id", "Title", selectedCourseId);

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
