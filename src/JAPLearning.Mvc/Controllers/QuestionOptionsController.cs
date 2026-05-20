using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Mvc.ViewModels.Entities;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class QuestionOptionsController : BaseController
    {
        private readonly IQuestionOptionService _service;
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionOptionsController(IQuestionOptionService service, IQuestionService questionService,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _questionService = questionService;
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
            await PopulateQuestionsAsync();
            return View(new QuestionOptionViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionOptionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questionoptions";
            if (!ModelState.IsValid) { await PopulateQuestionsAsync(); return View(vm); }
            var entity = _mapper.Map<QuestionOption>(vm);
            entity.Id = Guid.NewGuid();
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateQuestionsAsync(); return View(vm); }
            TempData["Success"] = "Opção criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "questionoptions";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateQuestionsAsync(entity.QuestionId);
            return View(_mapper.Map<QuestionOptionViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuestionOptionViewModel vm)
        {
            ViewData["ActiveMenu"] = "questionoptions";
            if (!ModelState.IsValid) { await PopulateQuestionsAsync(vm.QuestionId); return View(vm); }
            var entity = _mapper.Map<QuestionOption>(vm);
            entity.Id = id;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateQuestionsAsync(vm.QuestionId); return View(vm); }
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

        private async Task PopulateQuestionsAsync(Guid? selected = null)
        {
            ViewBag.Questions = new SelectList(await _questionService.GetAllAsync(), "Id", "Name", selected);
        }
    }
}
