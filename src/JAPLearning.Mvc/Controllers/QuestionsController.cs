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
    public class QuestionsController : BaseController
    {
        private readonly IQuestionService _service;
        private readonly ILessonService _lessonService;
        private readonly ITopicService _topicService;
        private readonly ICourseService _courseService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionService service, ILessonService lessonService,
            ITopicService topicService, ICourseService courseService, ITeamService teamService,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _lessonService = lessonService;
            _topicService = topicService;
            _courseService = courseService;
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "questions";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<QuestionViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "questions";
            await PopulateDropdownsAsync();
            return View(new QuestionViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questions";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(); return View(vm); }
            var entity = _mapper.Map<Question>(vm);
            entity.Id = Guid.NewGuid();
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(); return View(vm); }
            TempData["Success"] = "Questão criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "questions";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateDropdownsAsync(entity.LessonId);
            return View(_mapper.Map<QuestionViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuestionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questions";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(vm.LessonId); return View(vm); }
            var entity = _mapper.Map<Question>(vm);
            entity.Id = id;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(vm.LessonId); return View(vm); }
            TempData["Success"] = "Questão actualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Questão eliminada.";
            return RedirectToAction(nameof(Index));
        }

        // ── AJAX endpoints de cascata ─────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetCoursesByTeam(Guid teamId)
        {
            var allCourses     = await _courseService.GetAllAsync();
            var allCategories  = (await Task.FromResult(allCourses)).Select(c => c.Category).Distinct();
            var filtered = allCourses
                .Where(c => c.IsActived && c.Category != null && c.Category.TeamId == teamId)
                .OrderBy(c => c.Title)
                .Select(c => new { value = c.Id, text = c.Title });
            return Json(filtered);
        }

        [HttpGet]
        public async Task<IActionResult> GetTopicsByCourse(Guid courseId)
        {
            var all = await _topicService.GetAllAsync();
            var filtered = all
                .Where(t => t.IsActived && t.CourseId == courseId)
                .OrderBy(t => t.Order)
                .Select(t => new { value = t.Id, text = t.Name });
            return Json(filtered);
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonsByTopic(Guid topicId)
        {
            var all = await _lessonService.GetAllAsync();
            var filtered = all
                .Where(l => l.IsActived && l.TopicId == topicId)
                .OrderBy(l => l.Order)
                .Select(l => new { value = l.Id, text = l.Name });
            return Json(filtered);
        }

        // AJAX endpoint for QuestionOptions form
        [HttpGet]
        public async Task<IActionResult> GetByLesson(Guid lessonId)
        {
            var questions = await _service.GetByLessonAsync(lessonId);
            return Json(questions.Select(q => new { id = q.Id, name = q.Name }));
        }

        // ── Populate helpers ──────────────────────────────────────────────

        private async Task PopulateDropdownsAsync(Guid? selectedLessonId = null)
        {
            var teams = await _teamService.GetAllAsync();
            ViewBag.Teams = new SelectList(teams.Where(t => t.IsActived).OrderBy(t => t.Name), "Id", "Name");

            if (selectedLessonId.HasValue && selectedLessonId != Guid.Empty)
            {
                var lesson = await _lessonService.GetByIdAsync(selectedLessonId.Value);
                if (lesson != null)
                {
                    ViewBag.SelectedTopicId  = lesson.TopicId;
                    ViewBag.SelectedCourseId = lesson.CourseId;

                    // Determinar a equipa via categoria da formação
                    var course = await _courseService.GetByIdAsync(lesson.CourseId);
                    ViewBag.SelectedTeamId = course?.Category?.TeamId;
                }
            }
        }
    }
}
