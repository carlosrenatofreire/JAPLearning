using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Mvc.ViewModels.Parameters;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ModulesController : BaseController
    {
        private readonly IModuleService     _service;
        private readonly IPermissionService _permissionService;
        private readonly IMapper            _mapper;

        public ModulesController(IModuleService service, IPermissionService permissionService,
            IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service           = service;
            _permissionService = permissionService;
            _mapper            = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "modules";
            var modules     = await _service.GetAllAsync();
            var permissions = await _permissionService.GetAllAsync();

            var vms = _mapper.Map<List<ModuleViewModel>>(modules);
            foreach (var vm in vms)
                vm.PermissionCount = permissions.Count(p => p.ModuleId == vm.Id);

            return View(vms);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "modules";
            return View(new ModuleViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuleViewModel vm)
        {
            ViewData["ActiveMenu"] = "modules";
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Module>(vm);
            entity.Id  = Guid.NewGuid();

            if (!await _service.AddAsync(entity)) { AddErrors(); return View(vm); }

            TempData["Success"] = "Módulo criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "modules";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<ModuleViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ModuleViewModel vm)
        {
            ViewData["ActiveMenu"] = "modules";
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Module>(vm);
            entity.Id  = id;

            if (!await _service.UpdateAsync(entity)) { AddErrors(); return View(vm); }

            TempData["Success"] = "Módulo actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Módulo eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}
