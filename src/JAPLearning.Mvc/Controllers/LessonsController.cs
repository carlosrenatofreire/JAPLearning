using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Mvc.ViewModels.Entities;

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
            var list = await _service.GetAllAsync();
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
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

        private async Task PopulateDropdownsAsync(Guid? selectedCourseId = null)
        {
            var courses = await _courseService.GetAllAsync();
            ViewBag.Courses = new SelectList(courses.Where(c => c.IsActived), "Id", "Title", selectedCourseId);

            var topics = selectedCourseId.HasValue
                ? await _topicService.GetByCourseAsync(selectedCourseId.Value)
                : new List<Topic>();
            ViewBag.Topics = new SelectList(topics, "Id", "Name");

            var teams = await _teamService.GetAllAsync();
            ViewBag.Teams = new SelectList(teams.Where(t => t.IsActived), "Id", "Name");

            // Pre-select team for Edit
            if (selectedCourseId.HasValue)
            {
                var course = courses.FirstOrDefault(c => c.Id == selectedCourseId.Value);
                ViewBag.SelectedTeamId = course?.Category?.TeamId;
            }
        }
    }
}
