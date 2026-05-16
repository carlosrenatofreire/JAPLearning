using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class CertificatesController : BaseController
    {
        private readonly ICertificateService _service;
        private readonly IMapper _mapper;

        public CertificatesController(ICertificateService service, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "certificates";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<CertificateViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["ActiveMenu"] = "certificates";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<CertificateViewModel>(entity));
        }
    }
}
