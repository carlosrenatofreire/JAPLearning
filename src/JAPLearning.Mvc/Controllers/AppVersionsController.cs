using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Models.Domains.Auxiliaries;
using JAPLearning.Mvc.ViewModels.Auxiliaries;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize]
    public class AppVersionsController : BaseController
    {
        private readonly IAppVersionService _service;

        public AppVersionsController(IAppVersionService service, INotificator notificator)
            : base(notificator)
        {
            _service = service;
        }

        // ── Changelog (todos os utilizadores autenticados) ────────────────
        [HttpGet]
        public async Task<IActionResult> Changelog()
        {
            ViewData["ActiveMenu"] = "changelog";
            var versions = await _service.GetAllWithItemsAsync();
            return View(versions.Where(v => v.IsPublished).ToList());
        }

        // ── Index (só admin) ──────────────────────────────────────────────
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "versions";
            var list = await _service.GetAllWithItemsAsync();
            return View(list);
        }

        // ── Create ────────────────────────────────────────────────────────
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "versions";
            return View(new AppVersionViewModel { ReleaseDate = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppVersionViewModel vm)
        {
            ViewData["ActiveMenu"] = "versions";
            if (!ModelState.IsValid) return View(vm);

            var entity = new AppVersion
            {
                VersionNumber = vm.VersionNumber.Trim(),
                Title         = vm.Title.Trim(),
                ReleaseDate   = vm.ReleaseDate,
                IsPublished   = vm.IsPublished
            };

            if (!await _service.AddAsync(entity))
            {
                AddErrors();
                return View(vm);
            }

            TempData["Success"] = $"Versão {entity.VersionNumber} criada. Adicione os itens abaixo.";
            return RedirectToAction(nameof(Edit), new { id = entity.Id });
        }

        // ── Edit ──────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "versions";
            var entity = await _service.GetByIdWithItemsAsync(id);
            if (entity == null) return NotFound();

            var vm = MapToVm(entity);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AppVersionViewModel vm)
        {
            ViewData["ActiveMenu"] = "versions";
            if (!ModelState.IsValid)
            {
                var reload = await _service.GetByIdWithItemsAsync(id);
                if (reload != null) vm.Items = MapToVm(reload).Items;
                return View(vm);
            }

            var entity = new AppVersion
            {
                Id            = id,
                VersionNumber = vm.VersionNumber.Trim(),
                Title         = vm.Title.Trim(),
                ReleaseDate   = vm.ReleaseDate,
                IsPublished   = vm.IsPublished
            };

            if (!await _service.UpdateAsync(entity)) { AddErrors(); return View(vm); }

            TempData["Success"] = "Versão actualizada com sucesso.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        // ── Delete ────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Versão eliminada.";
            return RedirectToAction(nameof(Index));
        }

        // ── Items: Add ────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Guid versionId, VersionItemType type, string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                var existing = await _service.GetByIdWithItemsAsync(versionId);
                var nextOrder = existing?.Items.Any() == true ? existing.Items.Max(i => i.Order) + 1 : 0;

                var item = new AppVersionItem
                {
                    VersionId   = versionId,
                    Type        = type,
                    Description = description.Trim(),
                    Order       = nextOrder
                };
                await _service.AddItemAsync(item);
                TempData["Success"] = "Item adicionado.";
            }

            return RedirectToAction(nameof(Edit), new { id = versionId });
        }

        // ── Items: Delete ─────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(Guid itemId, Guid versionId)
        {
            await _service.DeleteItemAsync(itemId);
            TempData["Success"] = "Item removido.";
            return RedirectToAction(nameof(Edit), new { id = versionId });
        }

        // ── Helper ────────────────────────────────────────────────────────
        private static AppVersionViewModel MapToVm(AppVersion e) => new()
        {
            Id            = e.Id,
            VersionNumber = e.VersionNumber,
            Title         = e.Title,
            ReleaseDate   = e.ReleaseDate,
            IsPublished   = e.IsPublished,
            Items         = e.Items.OrderBy(i => i.Order).Select(i => new AppVersionItemViewModel
            {
                Id          = i.Id,
                VersionId   = i.VersionId,
                Type        = i.Type,
                Description = i.Description,
                Order       = i.Order
            }).ToList()
        };
    }
}
