using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JAPLearning.Business.Interfaces.Externals;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Mvc.ViewModels.Entities;
using System.Security.Claims;

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
        private readonly IWebHostEnvironment         _env;

        public CoursesController(ICourseService service, ICategoryService categoryService,
            ITeacherService teacherService, ILevelService levelService,
            ITeamService teamService, ILessonService lessonService,
            ICloudinaryService cloudinary, ICourseRequirementService requirementService,
            IMapper mapper, IWebHostEnvironment env, INotificator notificator) : base(notificator)
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
            _env                = env;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "courses";
            var allCourses    = await _service.GetAllAsync();
            var lessons       = await _lessonService.GetAllAsync();
            var teams         = await _teamService.GetAllAsync();
            var allCategories = await _categoryService.GetAllAsync();

            // ── Filtrar por departamento para Supervisor ─────────────────
            var isSupervisor = User.IsInRole("Supervisor");
            var teamIdClaim  = User.FindFirstValue("TeamId");
            var supervisorTeamId = isSupervisor && Guid.TryParse(teamIdClaim, out var tid) ? tid : (Guid?)null;

            var visibleCatIds = supervisorTeamId.HasValue
                ? allCategories.Where(c => c.TeamId == supervisorTeamId.Value).Select(c => c.Id).ToHashSet()
                : null;

            var list       = supervisorTeamId.HasValue
                ? allCourses.Where(c => visibleCatIds!.Contains(c.CategoryId)).ToList()
                : allCourses;

            var categories = supervisorTeamId.HasValue
                ? allCategories.Where(c => c.TeamId == supervisorTeamId.Value).ToList()
                : allCategories;

            // Equipas visíveis: Supervisor vê só a sua; Admin vê todas
            var visibleTeams = supervisorTeamId.HasValue
                ? teams.Where(t => t.Id == supervisorTeamId.Value).ToList()
                : teams.Where(t => t.IsActived).ToList();

            ViewBag.IsSupervisor = isSupervisor;
            ViewBag.SupervisorTeamId = supervisorTeamId?.ToString() ?? "";

            ViewBag.CourseDurations = lessons
                .Where(l => l.IsActived && l.TimeLesson.HasValue)
                .GroupBy(l => l.CourseId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Aggregate(TimeSpan.Zero, (sum, l) => sum + l.TimeLesson!.Value));

            ViewBag.FilterTeams = visibleTeams
                .OrderBy(t => t.Name)
                .Select(t => new { id = t.Id, name = t.Name })
                .ToList();

            ViewBag.FilterCategories = categories
                .Where(c => c.IsActived)
                .OrderBy(c => c.Name)
                .Select(c => new { id = c.Id, name = c.Name, teamId = c.TeamId })
                .ToList();

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
        [RequestSizeLimit(256 * 1024 * 1024)]           // 256 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 256 * 1024 * 1024)]
        public async Task<IActionResult> Create(CourseViewModel vm, IFormFile? thumbnail,
            IFormFile? snapshotFile, IFormFile? pdfFile)
        {
            ViewData["ActiveMenu"] = "courses";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(); return View(vm); }
            var entity = _mapper.Map<Course>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (thumbnail != null && thumbnail.Length > 0)
                entity.Thumbnail = await _cloudinary.UploadImageAsync(thumbnail, "courses");
            // ZIP → local (sem limite de tamanho, pode ultrapassar 100 MB)
            entity.SnapshotUrl = await SaveSnapshotLocalAsync(snapshotFile, entity.Id);
            // PDF → local (sem limite de tamanho)
            entity.PdfFileUrl = await SavePdfLocalAsync(pdfFile, entity.Id);
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
        [RequestSizeLimit(256 * 1024 * 1024)]           // 256 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 256 * 1024 * 1024)]
        public async Task<IActionResult> Edit(Guid id, CourseViewModel vm, IFormFile? thumbnail,
            IFormFile? snapshotFile, IFormFile? pdfFile)
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
            // ZIP → local: substitui ficheiro se novo enviado, mantém existente caso contrário
            var newSnapshot = await SaveSnapshotLocalAsync(snapshotFile, id);
            entity.SnapshotUrl = newSnapshot ?? vm.SnapshotUrl;

            // PDF → local: substitui ficheiro se novo enviado, mantém existente caso contrário
            var newPdf = await SavePdfLocalAsync(pdfFile, id);
            entity.PdfFileUrl = newPdf ?? vm.PdfFileUrl;

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

        /// <summary>
        /// Guarda o ficheiro ZIP do snapshot em PrivateFiles/snapshots/ (fora do wwwroot).
        /// Sem limite de tamanho — suporta ficheiros acima de 100 MB.
        /// Devolve o caminho relativo gravado na BD, ou null se não houver ficheiro.
        /// </summary>
        private async Task<string?> SavePdfLocalAsync(IFormFile? file, Guid courseId)
        {
            if (file == null || file.Length == 0) return null;

            var folder = Path.Combine(_env.ContentRootPath, "PrivateFiles", "pdfs");
            Directory.CreateDirectory(folder);

            var fileName = $"{courseId}.pdf";
            var fullPath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"pdfs/{fileName}";
        }

        private async Task<string?> SaveSnapshotLocalAsync(IFormFile? file, Guid courseId)
        {
            if (file == null || file.Length == 0) return null;

            var folder = Path.Combine(_env.ContentRootPath, "PrivateFiles", "snapshots");
            Directory.CreateDirectory(folder);

            // Um ficheiro por formação — {courseId}.zip sobrescreve o anterior automaticamente
            var fileName = $"{courseId}.zip";
            var fullPath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"snapshots/{fileName}";
        }

        private Guid? GetSupervisorTeamId()
        {
            if (!User.IsInRole("Supervisor")) return null;
            return Guid.TryParse(User.FindFirstValue("TeamId"), out var tid) ? tid : (Guid?)null;
        }

        private async Task PopulateDropdownsAsync(Guid? selectedCategoryId = null)
        {
            var supervisorTeamId = GetSupervisorTeamId();

            var allCategories = await _categoryService.GetAllAsync();
            var categories    = supervisorTeamId.HasValue
                ? allCategories.Where(c => c.IsActived && c.TeamId == supervisorTeamId.Value)
                : allCategories.Where(c => c.IsActived);

            ViewBag.CategoriesFull   = categories.OrderBy(c => c.Name).ToList();
            ViewBag.Categories       = new SelectList(categories.OrderBy(c => c.Name), "Id", "Name", selectedCategoryId);
            ViewBag.Teachers         = new SelectList(await _teacherService.GetAllAsync(), "Id", "Name");
            ViewBag.Levels           = new SelectList(await _levelService.GetAllAsync(), "Id", "Name");

            var allTeams = await _teamService.GetAllAsync();
            var teams    = supervisorTeamId.HasValue
                ? allTeams.Where(t => t.Id == supervisorTeamId.Value)
                : allTeams.Where(t => t.IsActived).OrderBy(t => t.Name);
            ViewBag.Teams            = new SelectList(teams, "Id", "Name");
            ViewBag.SupervisorTeamId = supervisorTeamId?.ToString() ?? "";
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
