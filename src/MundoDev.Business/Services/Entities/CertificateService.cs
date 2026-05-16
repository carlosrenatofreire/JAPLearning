using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class CertificateService : BaseService<Certificate, ICertificateRepository>, ICertificateService
    {
        public CertificateService(IUnitOfWork uow, ICertificateRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
