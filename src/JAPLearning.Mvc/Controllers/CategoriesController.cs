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
    [Authorize(Roles = "Administrador,Supervisor")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _service;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService service, ITeamService teamService, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _teamService = teamService;
            _mapper = mapper;
        }

        private async Task PopulateTeams(Guid? selectedId = null)
        {
            var teams = await _teamService.GetAllAsync();
            ViewBag.Teams = new SelectList(
                teams.Where(t => t.IsActived).OrderBy(t => t.Name),
                "Id", "Name", selectedId);
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "categories";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<CategoryViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "categories";
            await PopulateTeams();
            return View(new CategoryViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            ViewData["ActiveMenu"] = "categories";
            if (!ModelState.IsValid) { await PopulateTeams(vm.TeamId); return View(vm); }
            var entity = _mapper.Map<Category>(vm);
            entity.Id = Guid.NewGuid();
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateTeams(vm.TeamId); return View(vm); }
            TempData["Success"] = "Categoria criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "categories";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            var vm = _mapper.Map<CategoryViewModel>(entity);
            await PopulateTeams(vm.TeamId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CategoryViewModel vm)
        {
            ViewData["ActiveMenu"] = "categories";
            if (!ModelState.IsValid) { await PopulateTeams(vm.TeamId); return View(vm); }
            var entity = _mapper.Map<Category>(vm);
            entity.Id = id;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateTeams(vm.TeamId); return View(vm); }
            TempData["Success"] = "Categoria actualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Categoria eliminada.";
            return RedirectToAction(nameof(Index));
        }
    }
}
