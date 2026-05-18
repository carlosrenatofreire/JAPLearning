using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Externals;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Mvc.ViewModels.Parameters;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class TeachersController : BaseController
    {
        private readonly ITeacherService   _service;
        private readonly ICloudinaryService _cloudinary;
        private readonly IMapper           _mapper;

        public TeachersController(ITeacherService service, ICloudinaryService cloudinary,
            IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service    = service;
            _cloudinary = cloudinary;
            _mapper     = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "teachers";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<TeacherViewModel>>(list));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "teachers";
            return View(new TeacherViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel vm, IFormFile? photo)
        {
            ViewData["ActiveMenu"] = "teachers";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Teacher>(vm);
            entity.Id = Guid.NewGuid();
            if (photo != null && photo.Length > 0)
                entity.PhotoUrl = await _cloudinary.UploadImageAsync(photo, "teachers");
            if (!await _service.AddAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Professor criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "teachers";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<TeacherViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TeacherViewModel vm, IFormFile? photo)
        {
            ViewData["ActiveMenu"] = "teachers";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Teacher>(vm);
            entity.Id = id;
            if (photo != null && photo.Length > 0)
            {
                // Delete old photo from Cloudinary if exists
                if (!string.IsNullOrWhiteSpace(vm.PhotoUrl))
                {
                    var oldPublicId = _cloudinary.ExtractPublicId(vm.PhotoUrl);
                    if (oldPublicId != null) await _cloudinary.DeleteImageAsync(oldPublicId);
                }
                entity.PhotoUrl = await _cloudinary.UploadImageAsync(photo, "teachers");
            }
            if (!await _service.UpdateAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Professor actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Professor eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}
