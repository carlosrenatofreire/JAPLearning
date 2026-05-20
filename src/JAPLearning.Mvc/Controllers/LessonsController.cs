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
    public class LessonsController : BaseController
    {
        private readonly ILessonService _service;
        private readonly ICourseService _courseService;
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public LessonsController(ILessonService service, ICourseService courseService,
            ITopicService topicService, IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _courseService = courseService;
            _topicService = topicService;
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
            return View(new LessonViewModel { IsActived = true, Order = 1 });
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

        private async Task PopulateDropdownsAsync(Guid? selectedCourseId = null)
        {
            var courses = await _courseService.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title", selectedCourseId);

            // Load topics: if courseId provided load filtered, else load all
            var topics = selectedCourseId.HasValue
                ? await _topicService.GetByCourseAsync(selectedCourseId.Value)
                : await _topicService.GetAllAsync();
            ViewBag.Topics = new SelectList(topics, "Id", "Name");
        }
    }
}
