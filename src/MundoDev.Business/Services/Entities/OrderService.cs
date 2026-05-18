using Microsoft.AspNetCore.Http;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Auxiliaries;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Validators;

namespace MundoDev.Business.Services.Entities
{
    public class OrderService : AuditableService<Order, IOrderRepository>, IOrderService
    {
        public OrderService(
            IUnitOfWork uow,
            IOrderRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Order entity)
        {
            if (!Validate(new OrderValidator(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Order entity)
        {
            if (!Validate(new OrderValidator(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
