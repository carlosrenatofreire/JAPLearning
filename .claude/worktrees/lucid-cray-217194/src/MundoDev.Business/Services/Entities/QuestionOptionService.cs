using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class QuestionOptionService : BaseService<QuestionOption, IQuestionOptionRepository>, IQuestionOptionService
    {
        public QuestionOptionService(IUnitOfWork uow, IQuestionOptionRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
