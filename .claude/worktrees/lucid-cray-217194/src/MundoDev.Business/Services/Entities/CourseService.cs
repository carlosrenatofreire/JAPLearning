using Microsoft.AspNetCore.Http;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Auxiliaries;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Validators;

namespace MundoDev.Business.Services.Entities
{
    public class CourseService : AuditableService<Course, ICourseRepository>, ICourseService
    {
        public CourseService(
            IUnitOfWork uow,
            ICourseRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Course entity)
        {
            if (!Validate(new CourseValidator(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Course entity)
        {
            if (!Validate(new CourseValidator(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task<List<Course>> GetByCategoryAsync(Guid categoryId) =>
            (await _repository.Find(c => c.CategoryId == categoryId)).ToList();

        public async Task<List<Course>> GetByTeacherAsync(Guid teacherId) =>
            (await _repository.Find(c => c.TeacherId == teacherId)).ToList();
    }
}
