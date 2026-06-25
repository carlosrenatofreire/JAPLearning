using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class TestimonialsController : BaseController
    {
        private readonly ITestimonialService _service;
        private readonly IMapper _mapper;

        public TestimonialsController(ITestimonialService service, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _mapper = mapper;
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
        public async Task<IActionResult> Create(TestimonialViewModel vm)
        {
            ViewData["ActiveMenu"] = "testimonials";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Testimonial>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
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
        public async Task<IActionResult> Edit(Guid id, TestimonialViewModel vm)
        {
            ViewData["ActiveMenu"] = "testimonials";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<Testimonial>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
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
