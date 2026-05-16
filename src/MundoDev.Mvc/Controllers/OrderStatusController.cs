using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Mvc.ViewModels.Parameters;

namespace MundoDev.Mvc.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class OrderStatusController : BaseController
    {
        private readonly IOrderStatusService _service;
        private readonly IMapper _mapper;

        public OrderStatusController(IOrderStatusService service, IMapper mapper, INotificator notificator)
            : base(notificator)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "orderstatus";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<OrderStatusViewModel>>(list));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "orderstatus";
            return View(new OrderStatusViewModel { IsActived = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderStatusViewModel vm)
        {
            ViewData["ActiveMenu"] = "orderstatus";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<OrderStatus>(vm);
            entity.Id = Guid.NewGuid();
            if (!await _service.AddAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Estado de Pedido criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "orderstatus";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<OrderStatusViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrderStatusViewModel vm)
        {
            ViewData["ActiveMenu"] = "orderstatus";
            if (!ModelState.IsValid) return View(vm);
            var entity = _mapper.Map<OrderStatus>(vm);
            entity.Id = id;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); return View(vm); }
            TempData["Success"] = "Estado de Pedido actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Estado de Pedido eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}
