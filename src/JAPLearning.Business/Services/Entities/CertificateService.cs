using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class CertificateService : BaseService<Certificate, ICertificateRepository>, ICertificateService
    {
        public CertificateService(IUnitOfWork uow, ICertificateRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
