using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MundoDev.Business.Interfaces.Externals;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class CoursesController : BaseController
    {
        private readonly ICourseService   _service;
        private readonly ICategoryService _categoryService;
        private readonly ITeacherService  _teacherService;
        private readonly ILevelService    _levelService;
        private readonly ICloudinaryService _cloudinary;
        private readonly IMapper          _mapper;

        public CoursesController(ICourseService service, ICategoryService categoryService,
            ITeacherService teacherService, ILevelService levelService,
            ICloudinaryService cloudinary,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service         = service;
            _categoryService = categoryService;
            _teacherService  = teacherService;
            _levelService    = levelService;
            _cloudinary      = cloudinary;
            _mapper          = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "courses";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<CourseViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "courses";
            await PopulateDropdownsAsync();
            return View(new CourseViewModel { IsActived = true, PassingScore = 60 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel vm, IFormFile? thumbnail)
        {
            ViewData["ActiveMenu"] = "courses";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(); return View(vm); }
            var entity = _mapper.Map<Course>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (thumbnail != null && thumbnail.Length > 0)
                entity.Thumbnail = await _cloudinary.UploadImageAsync(thumbnail, "courses");
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(); return View(vm); }
            TempData["Success"] = "Curso criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "courses";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateDropdownsAsync();
            return View(_mapper.Map<CourseViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CourseViewModel vm, IFormFile? thumbnail)
        {
            ViewData["ActiveMenu"] = "courses";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(); return View(vm); }
            var entity = _mapper.Map<Course>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (thumbnail != null && thumbnail.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(vm.Thumbnail))
                {
                    var oldId = _cloudinary.ExtractPublicId(vm.Thumbnail);
                    if (oldId != null) await _cloudinary.DeleteImageAsync(oldId);
                }
                entity.Thumbnail = await _cloudinary.UploadImageAsync(thumbnail, "courses");
            }
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(); return View(vm); }
            TempData["Success"] = "Curso actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Curso eliminado.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            ViewBag.Teachers   = new SelectList(await _teacherService.GetAllAsync(), "Id", "Name");
            ViewBag.Levels     = new SelectList(await _levelService.GetAllAsync(), "Id", "Name");
        }
    }
}
