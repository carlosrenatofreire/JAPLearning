using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Enums;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class PlansController : BaseController
    {
        private readonly IPlanService _service;
        private readonly IMapper _mapper;

        public PlansController(IPlanService service, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "plans";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<PlanViewModel>>(list));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "plans";
            PopulatePlanTypes();
            return View(new PlanViewModel { IsActived = true, Price = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanViewModel vm)
        {
            ViewData["ActiveMenu"] = "plans";
            if (!ModelState.IsValid) { PopulatePlanTypes(); return View(vm); }
            var entity = _mapper.Map<Plan>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (!await _service.AddAsync(entity)) { AddErrors(); PopulatePlanTypes(); return View(vm); }
            TempData["Success"] = "Plano criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "plans";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            PopulatePlanTypes(entity.PlanType);
            return View(_mapper.Map<PlanViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PlanViewModel vm)
        {
            ViewData["ActiveMenu"] = "plans";
            if (!ModelState.IsValid) { PopulatePlanTypes(vm.PlanType); return View(vm); }
            var entity = _mapper.Map<Plan>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); PopulatePlanTypes(vm.PlanType); return View(vm); }
            TempData["Success"] = "Plano actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Plano eliminado.";
            return RedirectToAction(nameof(Index));
        }

        private void PopulatePlanTypes(PlanType? selected = null)
        {
            ViewBag.PlanTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new() { Value = "1", Text = "Semestral", Selected = selected == PlanType.SemiAnnual },
                new() { Value = "2", Text = "Anual", Selected = selected == PlanType.Annual },
                new() { Value = "3", Text = "A Pedido", Selected = selected == PlanType.OnRequest },
            };
        }
    }
}
