using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class TestimonialService : AuditableService<Testimonial, ITestimonialRepository>, ITestimonialService
    {
        public TestimonialService(
            IUnitOfWork uow,
            ITestimonialRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Testimonial entity)
        {
            if (!await ValidateAsync(new TestimonialValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Testimonial entity)
        {
            if (!await ValidateAsync(new TestimonialValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
