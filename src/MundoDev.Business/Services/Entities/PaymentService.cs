using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class PaymentService : BaseService<Payment, IPaymentRepository>, IPaymentService
    {
        public PaymentService(IUnitOfWork uow, IPaymentRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
