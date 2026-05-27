using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Externals;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Mvc.ViewModels.Parameters;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class TeamsController : BaseController
    {
        private readonly ITeamService        _service;
        private readonly ICloudinaryService  _cloudinary;
        private readonly IMapper             _mapper;

        public TeamsController(ITeamService service, ICloudinaryService cloudinary,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service    = service;
            _cloudinary = cloudinary;
            _mapper     = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "teams";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<TeamViewModel>>(list));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "teams";
            return View(new TeamViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamViewModel vm, IFormFile? thumbnail)
        {
            ViewData["ActiveMenu"] = "teams";
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Team>(vm);
            entity.Id = Guid.NewGuid();

            if (thumbnail != null && thumbnail.Length > 0)
                entity.Thumbnail = await _cloudinary.UploadImageAsync(thumbnail, "teams");

            if (!await _service.AddAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Equipa criada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "teams";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<TeamViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TeamViewModel vm, IFormFile? thumbnail)
        {
            ViewData["ActiveMenu"] = "teams";
            if (!ModelState.IsValid) return View(vm);

            var existing = await _service.GetByIdAsync(id);
            var entity   = _mapper.Map<Team>(vm);
            entity.Id    = id;

            if (thumbnail != null && thumbnail.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(existing?.Thumbnail))
                {
                    var oldId = _cloudinary.ExtractPublicId(existing.Thumbnail);
                    if (oldId != null) await _cloudinary.DeleteImageAsync(oldId);
                }
                entity.Thumbnail = await _cloudinary.UploadImageAsync(thumbnail, "teams");
            }
            else
            {
                entity.Thumbnail = existing?.Thumbnail;
            }

            if (!await _service.UpdateAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Equipa actualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (!string.IsNullOrWhiteSpace(existing?.Thumbnail))
            {
                var oldId = _cloudinary.ExtractPublicId(existing.Thumbnail);
                if (oldId != null) await _cloudinary.DeleteImageAsync(oldId);
            }
            await _service.DeleteAsync(id);
            TempData["Success"] = "Equipa eliminada.";
            return RedirectToAction(nameof(Index));
        }
    }
}
