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
    public class TopicsController : BaseController
    {
        private readonly ITopicService _service;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public TopicsController(ITopicService service, ICourseService courseService,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _courseService = courseService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "topics";
            var list = await _service.GetAllAsync();
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
            if (!ModelState.IsValid) { await PopulateCoursesAsync(); return View(vm); }
            var entity = _mapper.Map<Topic>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateCoursesAsync(); return View(vm); }
            TempData["Success"] = "Tópico criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "topics";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateCoursesAsync();
            return View(_mapper.Map<TopicViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TopicViewModel vm)
        {
            ViewData["ActiveMenu"] = "topics";
            if (!ModelState.IsValid) { await PopulateCoursesAsync(); return View(vm); }
            var entity = _mapper.Map<Topic>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateCoursesAsync(); return View(vm); }
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

        // AJAX endpoint — returns topics for a given course as JSON
        [HttpGet]
        public async Task<IActionResult> GetByCourse(Guid courseId)
        {
            var topics = await _service.GetByCourseAsync(courseId);
            return Json(topics.Select(t => new { t.Id, t.Name }));
        }

        private async Task PopulateCoursesAsync()
        {
            ViewBag.Courses = new SelectList(await _courseService.GetAllAsync(), "Id", "Title");
        }
    }
}
