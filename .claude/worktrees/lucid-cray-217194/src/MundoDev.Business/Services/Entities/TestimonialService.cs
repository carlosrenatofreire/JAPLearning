using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class TestimonialService : BaseService<Testimonial, ITestimonialRepository>, ITestimonialService
    {
        public TestimonialService(IUnitOfWork uow, ITestimonialRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
