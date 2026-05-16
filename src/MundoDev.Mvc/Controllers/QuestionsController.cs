using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class QuestionsController : BaseController
    {
        private readonly IQuestionService _service;
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionService service, ILessonService lessonService,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _lessonService = lessonService;
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
            await PopulateLessonsAsync();
            return View(new QuestionViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questions";
            if (!ModelState.IsValid) { await PopulateLessonsAsync(); return View(vm); }
            var entity = _mapper.Map<Question>(vm);
            entity.Id = Guid.NewGuid();
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateLessonsAsync(); return View(vm); }
            TempData["Success"] = "Questão criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "questions";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateLessonsAsync(entity.LessonId);
            return View(_mapper.Map<QuestionViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuestionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questions";
            if (!ModelState.IsValid) { await PopulateLessonsAsync(vm.LessonId); return View(vm); }
            var entity = _mapper.Map<Question>(vm);
            entity.Id = id;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateLessonsAsync(vm.LessonId); return View(vm); }
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

        // AJAX endpoint for QuestionOptions form
        [HttpGet]
        public async Task<IActionResult> GetByLesson(Guid lessonId)
        {
            var questions = await _service.GetByLessonAsync(lessonId);
            return Json(questions.Select(q => new { id = q.Id, name = q.Name }));
        }

        private async Task PopulateLessonsAsync(Guid? selected = null)
        {
            ViewBag.Lessons = new SelectList(await _lessonService.GetAllAsync(), "Id", "Name", selected);
        }
    }
}
