using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UsersController(IUserService service, IRoleService roleService, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "users";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<UserViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "users";
            await PopulateRolesAsync();
            return View(new UserViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel vm)
        {
            ViewData["ActiveMenu"] = "users";
            if (string.IsNullOrWhiteSpace(vm.Password))
                ModelState.AddModelError("Password", "A senha é obrigatória para novos utilizadores.");

            if (!ModelState.IsValid) { await PopulateRolesAsync(); return View(vm); }

            var entity = _mapper.Map<User>(vm);
            entity.Id = Guid.NewGuid();
            entity.Password = BCrypt.Net.BCrypt.HashPassword(vm.Password);
            entity.CreatedDate = DateTime.UtcNow;

            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateRolesAsync(); return View(vm); }
            TempData["Success"] = "Utilizador criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "users";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateRolesAsync();
            return View(_mapper.Map<UserViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserViewModel vm)
        {
            ViewData["ActiveMenu"] = "users";
            // Remove password validation on edit (optional change)
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid) { await PopulateRolesAsync(); return View(vm); }

            var entity = _mapper.Map<User>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;

            // Only hash and update password if provided
            if (!string.IsNullOrWhiteSpace(vm.Password))
                entity.Password = BCrypt.Net.BCrypt.HashPassword(vm.Password);
            else
            {
                // Keep existing password — fetch from DB
                var existing = await _service.GetByIdAsync(id);
                entity.Password = existing?.Password ?? string.Empty;
            }

            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateRolesAsync(); return View(vm); }
            TempData["Success"] = "Utilizador actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Utilizador eliminado.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateRolesAsync()
        {
            var roles = await _roleService.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
        }
    }
}
