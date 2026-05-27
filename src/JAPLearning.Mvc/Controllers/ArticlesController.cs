using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JAPLearning.Business.Interfaces.Externals;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Mvc.ViewModels.Entities;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class ArticlesController : BaseController
    {
        private readonly IArticleService _service;
        private readonly ISubjectService _subjectService;
        private readonly ICloudinaryService _cloudinary;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService service, ISubjectService subjectService,
            ICloudinaryService cloudinary, IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _subjectService = subjectService;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "articles";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<ArticleViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "articles";
            await PopulateSubjectsAsync();
            return View(new ArticleViewModel
            {
                PublishDate = DateTime.Today,
                IsActived = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel vm, IFormFile? coverImageFile)
        {
            ViewData["ActiveMenu"] = "articles";
            if (!ModelState.IsValid) { await PopulateSubjectsAsync(); return View(vm); }
            var entity = _mapper.Map<Article>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(entity.Slug))
                entity.Slug = GenerateSlug(entity.Name);
            if (coverImageFile != null && coverImageFile.Length > 0)
                entity.CoverImage = await _cloudinary.UploadImageAsync(coverImageFile, "articles");
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateSubjectsAsync(); return View(vm); }
            TempData["Success"] = "Artigo criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "articles";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateSubjectsAsync(entity.SubjectId);
            return View(_mapper.Map<ArticleViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ArticleViewModel vm, IFormFile? coverImageFile)
        {
            ViewData["ActiveMenu"] = "articles";
            if (!ModelState.IsValid) { await PopulateSubjectsAsync(vm.SubjectId); return View(vm); }
            var entity = _mapper.Map<Article>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(entity.Slug))
                entity.Slug = GenerateSlug(entity.Name);
            if (coverImageFile != null && coverImageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(vm.CoverImage))
                {
                    var oldId = _cloudinary.ExtractPublicId(vm.CoverImage);
                    if (oldId != null) await _cloudinary.DeleteImageAsync(oldId);
                }
                entity.CoverImage = await _cloudinary.UploadImageAsync(coverImageFile, "articles");
            }
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateSubjectsAsync(vm.SubjectId); return View(vm); }
            TempData["Success"] = "Artigo actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Artigo eliminado.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSubjectsAsync(Guid? selected = null)
        {
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllAsync(), "Id", "Name", selected);
        }

        private static string GenerateSlug(string name)
        {
            return name.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("ã", "a").Replace("á", "a").Replace("à", "a").Replace("â", "a")
                .Replace("é", "e").Replace("ê", "e").Replace("í", "i")
                .Replace("ó", "o").Replace("ô", "o").Replace("õ", "o")
                .Replace("ú", "u").Replace("ç", "c")
                + "-" + DateTime.UtcNow.Ticks.ToString()[^6..];
        }
    }
}
