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
    public class OrdersController : BaseController
    {
        private readonly IOrderService _service;
        private readonly IUserService _userService;
        private readonly IPlanService _planService;
        private readonly IOrderStatusService _statusService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService service, IUserService userService,
            IPlanService planService, IOrderStatusService statusService,
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _service = service;
            _userService = userService;
            _planService = planService;
            _statusService = statusService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "orders";
            var list = await _service.GetAllAsync();
            return View(_mapper.Map<List<OrderViewModel>>(list));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ActiveMenu"] = "orders";
            await PopulateDropdownsAsync();
            return View(new OrderViewModel
            {
                StartLicense = DateTime.Today,
                EndLicense = DateTime.Today.AddMonths(12),
                IsActived = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel vm)
        {
            ViewData["ActiveMenu"] = "orders";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(); return View(vm); }
            var entity = _mapper.Map<Order>(vm);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            if (!await _service.AddAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(); return View(vm); }
            TempData["Success"] = "Pedido criado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["ActiveMenu"] = "orders";
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await PopulateDropdownsAsync(entity.UserId, entity.PlanId, entity.StatusId);
            return View(_mapper.Map<OrderViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrderViewModel vm)
        {
            ViewData["ActiveMenu"] = "orders";
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(vm.UserId, vm.PlanId, vm.StatusId); return View(vm); }
            var entity = _mapper.Map<Order>(vm);
            entity.Id = id;
            entity.ChangedDate = DateTime.UtcNow;
            if (!await _service.UpdateAsync(entity)) { AddErrors(); await PopulateDropdownsAsync(vm.UserId, vm.PlanId, vm.StatusId); return View(vm); }
            TempData["Success"] = "Pedido actualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Pedido eliminado.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(Guid? userId = null, Guid? planId = null, Guid? statusId = null)
        {
            var users = await _userService.GetAllAsync();
            ViewBag.Users = new SelectList(
                users.Select(u => new { u.Id, FullName = $"{u.FirstName} {u.LastName}" }),
                "Id", "FullName", userId);
            ViewBag.Plans = new SelectList(await _planService.GetAllAsync(), "Id", "Name", planId);
            ViewBag.Statuses = new SelectList(await _statusService.GetAllAsync(), "Id", "Name", statusId);
        }
    }
}
