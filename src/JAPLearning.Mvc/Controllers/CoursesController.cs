using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JAPLearning.Business.Interfaces.Externals;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Mvc.ViewModels.Entities;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class CoursesController : BaseController
    {
        private readonly ICourseService              _service;
        private readonly ICategoryService            _categoryService;
        private readonly ITeacherService             _teacherService;
        private readonly ILevelService               _levelService;
        private readonly ITeamService                _teamService;
        private readonly ILessonService              _lessonService;
        private readonly ICloudinaryService          _cloudinary;
        private readonly ICourseRequirementService   _requirementService;
        private readonly IMapper                     _mapper;

        public CoursesController(ICourseService service, ICategoryService categoryService,
            ITeacherService teacherService, ILevelService levelService,
            ITeamService teamService, ILessonService lessonService,
            ICloudinaryService cloudinary, ICourseRequirementService requirementService,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service            = service;
            _categoryService    = categoryService;
            _teacherService     = teacherService;
            _levelService       = levelService;
            _teamService        = teamService;
            _lessonService      = lessonService;
            _cloudinary         = cloudinary;
            _requirementService = requirementService;
            _mapper             = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "courses";
            var list    = await _service.GetAllAsync();
            var lessons = await _lessonService.GetAllAsync();

            ViewBag.CourseDurations = lessons
                .Where(l => l.IsActived && l.TimeLesson.HasValue)
                .GroupBy(l => l.CourseId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Aggregate(TimeSpan.Zero, (sum, l) => sum + l.TimeLesson!.Value));

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
            await PopulateDropdownsAsync(entity.CategoryId);
            await PopulateRequirementsAsync(id);
            // Pre-select the team based on the current category
            var categories = await _categoryService.GetAllAsync();
            var currentCat = categories.FirstOrDefault(c => c.Id == entity.CategoryId);
            ViewBag.SelectedTeamId = currentCat?.TeamId;
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

        // ── Pré-requisitos ──────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRequirement(Guid courseId, Guid prerequisiteCourseId)
        {
            if (prerequisiteCourseId == Guid.Empty)
            {
                TempData["Error"] = "Seleccione um curso para adicionar como pré-requisito.";
                return RedirectToAction(nameof(Edit), new { id = courseId });
            }

            var added = await _requirementService.AddAsync(courseId, prerequisiteCourseId);
            TempData[added ? "Success" : "Error"] = added
                ? "Pré-requisito adicionado com sucesso."
                : "Pré-requisito já existe ou é inválido.";

            return RedirectToAction(nameof(Edit), new { id = courseId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRequirement(Guid requirementId, Guid courseId)
        {
            await _requirementService.RemoveAsync(requirementId);
            TempData["Success"] = "Pré-requisito removido.";
            return RedirectToAction(nameof(Edit), new { id = courseId });
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private async Task PopulateDropdownsAsync(Guid? selectedCategoryId = null)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.CategoriesFull = categories.Where(c => c.IsActived)
                                               .OrderBy(c => c.Name).ToList();
            ViewBag.Categories     = new SelectList(categories.Where(c => c.IsActived).OrderBy(c => c.Name), "Id", "Name", selectedCategoryId);
            ViewBag.Teachers       = new SelectList(await _teacherService.GetAllAsync(), "Id", "Name");
            ViewBag.Levels         = new SelectList(await _levelService.GetAllAsync(), "Id", "Name");
            ViewBag.Teams          = new SelectList((await _teamService.GetAllAsync()).Where(t => t.IsActived).OrderBy(t => t.Name), "Id", "Name");
        }

        private async Task PopulateRequirementsAsync(Guid courseId)
        {
            var requirements = await _requirementService.GetByCourseAsync(courseId);
            var allCourses   = await _service.GetAllAsync();
            var coursesById  = allCourses.ToDictionary(c => c.Id);

            // Enriquece a navegação manualmente (o repositório base não faz Include)
            foreach (var req in requirements)
                if (coursesById.TryGetValue(req.PrerequisiteCourseId, out var prereq))
                    req.PrerequisiteCourse = prereq;

            // Determina a equipa da formação actual (via Category.TeamId)
            var currentCourse = coursesById.GetValueOrDefault(courseId);
            var currentTeamId = currentCourse?.Category?.TeamId;

            // Cursos disponíveis: mesma equipa, activos, excluindo o próprio e pré-requisitos já existentes
            var existingIds = requirements.Select(r => r.PrerequisiteCourseId).ToHashSet();
            existingIds.Add(courseId);

            var available = allCourses
                .Where(c => c.IsActived
                         && !existingIds.Contains(c.Id)
                         && (currentTeamId == null || c.Category?.TeamId == currentTeamId))
                .OrderBy(c => c.Title);

            ViewBag.Requirements     = requirements;
            ViewBag.AvailableCourses = new SelectList(available, "Id", "Title");
        }
    }
}
