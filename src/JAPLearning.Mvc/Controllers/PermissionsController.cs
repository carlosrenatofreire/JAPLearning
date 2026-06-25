using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Mvc.ViewModels.Parameters;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PermissionsController : BaseController
    {
        private readonly IPermissionService _service;
        private readonly IModuleService     _moduleService;
        private readonly IMapper            _mapper;

        public PermissionsController(IPermissionService service, IModuleService moduleService,
            IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service       = service;
            _moduleService = moduleService;
            _mapper        = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "permissions";
            var permissions = await _service.GetAllAsync();
            var modules     = await _moduleService.GetAllAsync();

            var vms = _mapper.Map<List<PermissionViewModel>>(permissions);
            var moduleMap = modules.ToDictionary(m => m.Id, m => m.Name);
            foreach (var vm in vms)
                vm.ModuleName = moduleMap.TryGetValue(vm.ModuleId, out var n) ? n : "—";

            return View(vms);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "permissions";
            await PopulateModules();
            return View(new PermissionViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PermissionViewModel vm)
        {
            ViewData["ActiveMenu"] = "permissions";
            if (!ModelState.IsValid) { await PopulateModules(vm.ModuleId); return View(vm); }

            var entity = _mapper.Map<Permission>(vm);
            entity.Id  = Guid.NewGuid();

            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateModules(vm.ModuleId); return View(vm); }

            TempData["Success"] = "Permissão criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "permissions";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateModules(entity.ModuleId);
            return View(_mapper.Map<PermissionViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PermissionViewModel vm)
        {
            ViewData["ActiveMenu"] = "permissions";
            if (!ModelState.IsValid) { await PopulateModules(vm.ModuleId); return View(vm); }

            var entity = _mapper.Map<Permission>(vm);
            entity.Id  = id;

            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateModules(vm.ModuleId); return View(vm); }

            TempData["Success"] = "Permissão actualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Permissão eliminada.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateModules(Guid? selected = null)
        {
            var modules = await _moduleService.GetAllAsync();
            ViewBag.Modules = new SelectList(
                modules.Where(m => m.IsActived).OrderBy(m => m.Name),
                "Id", "Name", selected);
        }
    }
}
