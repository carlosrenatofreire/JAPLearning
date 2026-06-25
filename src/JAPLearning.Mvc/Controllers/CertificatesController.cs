using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Mvc.ViewModels.Entities;
using System.Security.Claims;

namespace JAPLearning.Mvc.Controllers
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

        private Guid? GetSupervisorTeamId()
        {
            if (!User.IsInRole("Supervisor")) return null;
            return Guid.TryParse(User.FindFirstValue("TeamId"), out var tid) ? tid : (Guid?)null;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "certificates";
            var supervisorTeamId = GetSupervisorTeamId();

            var all = await _service.GetAllAsync();
            var list = supervisorTeamId.HasValue
                ? all.Where(c => c.Course?.Category?.TeamId == supervisorTeamId.Value).ToList()
                : all;

            return View(_mapper.Map<List<CertificateViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["ActiveMenu"] = "certificates";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var supervisorTeamId = GetSupervisorTeamId();
            if (supervisorTeamId.HasValue && entity.Course?.Category?.TeamId != supervisorTeamId.Value)
                return Forbid();

            return View(_mapper.Map<CertificateViewModel>(entity));
        }
    }
}
