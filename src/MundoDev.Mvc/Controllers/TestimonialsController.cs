using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Mvc.Services;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class TestimonialsController : BaseController
    {
        private readonly ITestimonialService _service;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinary;

        public TestimonialsController(ITestimonialService service, IMapper mapper,
            INotificator notificator, ICloudinaryService cloudinary)
            : base(notificator)
        {
            _service   = service;
            _mapper    = mapper;
            _cloudinary = cloudinary;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "testimonials";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<TestimonialViewModel>>(list));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "testimonials";
            return View(new TestimonialViewModel { IsActived = true, Rating = 5 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialViewModel vm, IFormFile? photo)
        {
            ViewData["ActiveMenu"] = "testimonials";
            if (photo != null && photo.Length > 0) ModelState.Remove(nameof(vm.PhotoUrl));
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Testimonial>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (photo != null && photo.Length > 0)
                entity.PhotoUrl = await _cloudinary.UploadImageAsync(photo, "testimonials");
            if (!await _service.AddAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Testemunho criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "testimonials";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<TestimonialViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TestimonialViewModel vm, IFormFile? photo)
        {
            ViewData["ActiveMenu"] = "testimonials";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Testimonial>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (photo != null && photo.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(vm.PhotoUrl))
                {
                    var oldId = _cloudinary.ExtractPublicId(vm.PhotoUrl);
                    if (oldId != null) await _cloudinary.DeleteImageAsync(oldId);
                }
                entity.PhotoUrl = await _cloudinary.UploadImageAsync(photo, "testimonials");
            }
            else
            {
                entity.PhotoUrl = vm.PhotoUrl;
            }
            if (!await _service.UpdateAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Testemunho actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Testemunho eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}
