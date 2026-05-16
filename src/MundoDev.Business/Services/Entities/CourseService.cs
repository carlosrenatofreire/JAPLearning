using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class CourseService : BaseService<Course, ICourseRepository>, ICourseService
    {
        public CourseService(IUnitOfWork uow, ICourseRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public async Task<List<Course>> GetByCategoryAsync(Guid categoryId) =>
            (await _repository.Find(c => c.CategoryId == categoryId)).ToList();

        public async Task<List<Course>> GetByTeacherAsync(Guid teacherId) =>
            (await _repository.Find(c => c.TeacherId == teacherId)).ToList();
    }
}
