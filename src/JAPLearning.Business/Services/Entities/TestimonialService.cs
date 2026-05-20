using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class TestimonialService : BaseService<Testimonial, ITestimonialRepository>, ITestimonialService
    {
        public TestimonialService(IUnitOfWork uow, ITestimonialRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
