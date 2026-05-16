using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Mvc.ViewModels.Parameters;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class LevelsController : BaseController
    {
        private readonly ILevelService _service;
        private readonly IMapper _mapper;

        public LevelsController(ILevelService service, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "levels";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<LevelViewModel>>(list));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "levels";
            return View(new LevelViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LevelViewModel vm)
        {
            ViewData["ActiveMenu"] = "levels";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Level>(vm);
            entity.Id = Guid.NewGuid();
            if (!await _service.AddAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Nível criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "levels";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<LevelViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LevelViewModel vm)
        {
            ViewData["ActiveMenu"] = "levels";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Level>(vm);
            entity.Id = id;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Nível actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Nível eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}
