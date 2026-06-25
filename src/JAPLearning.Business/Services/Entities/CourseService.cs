using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

namespace JAPLearning.Business.Services.Entities
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
            if (!await ValidateAsync(new CourseValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Course entity)
        {
            if (!await ValidateAsync(new CourseValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task<List<Course>> GetByCategoryAsync(Guid categoryId) =>
            (await _repository.Find(c => c.CategoryId == categoryId)).ToList();

        public async Task<List<Course>> GetByTeacherAsync(Guid teacherId) =>
            (await _repository.Find(c => c.TeacherId == teacherId)).ToList();
    }
}
