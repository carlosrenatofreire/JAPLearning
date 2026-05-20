using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Mvc.ViewModels.Entities;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _service;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService service, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "payments";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<PaymentViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["ActiveMenu"] = "payments";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<PaymentViewModel>(entity));
        }
    }
}
