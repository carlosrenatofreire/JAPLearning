using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Services.Parameters
{
    public class SubjectService : BaseService<Subject, ISubjectRepository>, ISubjectService
    {
        public SubjectService(IUnitOfWork uow, ISubjectRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
