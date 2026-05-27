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
    public class QuestionOptionsController : BaseController
    {
        private readonly IQuestionOptionService _service;
        private readonly IQuestionService _questionService;
        private readonly ILessonService _lessonService;
        private readonly ITopicService _topicService;
        private readonly ICourseService _courseService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public QuestionOptionsController(IQuestionOptionService service, IQuestionService questionService,
            ILessonService lessonService, ITopicService topicService, ICourseService courseService,
            ITeamService teamService, IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _questionService = questionService;
            _lessonService = lessonService;
            _topicService = topicService;
            _courseService = courseService;
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "questionoptions";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<QuestionOptionViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "questionoptions";
            await PopulateDropdownsAsync();
            return View(new QuestionOptionViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionOptionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questionoptions";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(); return View(vm); }
            var entity = _mapper.Map<QuestionOption>(vm);
            entity.Id = Guid.NewGuid();
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(); return View(vm); }
            TempData["Success"] = "Opção criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "questionoptions";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateDropdownsAsync(entity.QuestionId);
            return View(_mapper.Map<QuestionOptionViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuestionOptionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questionoptions";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(vm.QuestionId); return View(vm); }
            var entity = _mapper.Map<QuestionOption>(vm);
            entity.Id = id;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(vm.QuestionId); return View(vm); }
            TempData["Success"] = "Opção actualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Opção eliminada.";
            return RedirectToAction(nameof(Index));
        }

        // ── AJAX cascata ─────────────────────────────────────────────────

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

        [HttpGet]
        public async Task<IActionResult> GetQuestionsByLesson(Guid lessonId)
        {
            var all = await _questionService.GetByLessonAsync(lessonId);
            var filtered = all
                .Where(q => q.IsActived)
                .OrderBy(q => q.Name)
                .Select(q => new { value = q.Id, text = q.Name });
            return Json(filtered);
        }

        // ── Helper ───────────────────────────────────────────────────────

        private async Task PopulateDropdownsAsync(Guid? selectedQuestionId = null)
        {
            var teams = await _teamService.GetAllAsync();
            ViewBag.Teams = new SelectList(teams.Where(t => t.IsActived).OrderBy(t => t.Name), "Id", "Name");

            if (selectedQuestionId.HasValue && selectedQuestionId != Guid.Empty)
            {
                var question = await _questionService.GetByIdAsync(selectedQuestionId.Value);
                if (question != null)
                {
                    ViewBag.SelectedLessonId   = question.LessonId;
                    var lesson = await _lessonService.GetByIdAsync(question.LessonId);
                    if (lesson != null)
                    {
                        ViewBag.SelectedTopicId  = lesson.TopicId;
                        ViewBag.SelectedCourseId = lesson.CourseId;
                        var course = await _courseService.GetByIdAsync(lesson.CourseId);
                        ViewBag.SelectedTeamId = course?.Category?.TeamId;
                    }
                }
            }
        }
    }
}
