using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Models.Domains.Auxiliaries;
using JAPLearning.Business.Models.Enums;
using JAPLearning.Data.Contexts;
using JAPLearning.Mvc.ViewModels.Auxiliaries;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AuditLogsController : BaseController
    {
        private readonly IAuditLogService _service;
        private readonly IMapper          _mapper;
        private readonly MainDbContext    _db;

        public AuditLogsController(IAuditLogService service, IMapper mapper, INotificator notificator, MainDbContext db)
            : base(notificator)
        {
            _service = service;
            _mapper  = mapper;
            _db      = db;
        }

        public async Task<IActionResult> Index(
            string? search,
            string? level,
            string? actionFilter,   // ← NÃO usar "action" — conflito com route value do MVC
            string? entity,
            DateTime? from,
            DateTime? to)
        {
            ViewData["ActiveMenu"] = "auditlogs";

            var all = await _db.AuditLogs
                .AsNoTracking()
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();

            // Filtros em memória
            var result = all.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
                result = result.Where(l =>
                    (l.Message    != null && l.Message.Contains(search,    StringComparison.OrdinalIgnoreCase)) ||
                    (l.CreatedBy  != null && l.CreatedBy.Contains(search,  StringComparison.OrdinalIgnoreCase)) ||
                    (l.EntityName != null && l.EntityName.Contains(search, StringComparison.OrdinalIgnoreCase)));

            if (!string.IsNullOrWhiteSpace(level) && Enum.TryParse<LogType>(level, out var logLevel))
                result = result.Where(l => l.LogLevel == logLevel);

            if (!string.IsNullOrWhiteSpace(actionFilter))
                result = result.Where(l => l.Action != null && l.Action.Contains(actionFilter, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(entity))
                result = result.Where(l => l.EntityName != null && l.EntityName.Equals(entity, StringComparison.OrdinalIgnoreCase));

            if (from.HasValue)
                result = result.Where(l => l.CreatedDate >= from.Value);

            if (to.HasValue)
                result = result.Where(l => l.CreatedDate <= to.Value.AddDays(1));

            var finalList = result.ToList();

            var mapped = finalList.Select(log => new AuditLogViewModel
            {
                Id             = log.Id,
                LogLevel       = log.LogLevel,
                CreatedDate    = log.CreatedDate,
                CreatedBy      = log.CreatedBy,
                Action         = log.Action,
                EntityName     = log.EntityName,
                HttpStatusCode = log.HttpStatusCode,
                Message        = log.Message,
                StackTrace     = log.StackTrace,
                Json           = log.Json
            }).ToList();

            ViewBag.TotalInDb    = all.Count;
            ViewBag.Entities     = all.Where(l => l.EntityName != null).Select(l => l.EntityName!).Distinct().OrderBy(x => x).ToList();
            ViewBag.Search       = search;
            ViewBag.FilterLevel  = level;
            ViewBag.FilterAction = actionFilter;
            ViewBag.FilterEntity = entity;
            ViewBag.FilterFrom   = from?.ToString("yyyy-MM-dd");
            ViewBag.FilterTo     = to?.ToString("yyyy-MM-dd");

            return View(mapped);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["ActiveMenu"] = "auditlogs";
            var log = await _db.AuditLogs.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            if (log == null) return NotFound();
            return View(_mapper.Map<AuditLogViewModel>(log));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var all = await _db.AuditLogs.ToListAsync();
            _db.AuditLogs.RemoveRange(all);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Registos de auditoria limpos.";
            return RedirectToAction(nameof(Index));
        }
    }
}
