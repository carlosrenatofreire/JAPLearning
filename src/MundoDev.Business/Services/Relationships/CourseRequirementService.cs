using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Relationships;
using MundoDev.Business.Models.Domains.Relationships;

namespace MundoDev.Business.Services.Relationships
{
    public class CourseRequirementService : ICourseRequirementService
    {
        private readonly ICourseRequirementRepository _repository;
        private readonly IUnitOfWork _uow;

        public CourseRequirementService(ICourseRequirementRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow        = uow;
        }

        public async Task<List<CourseRequirement>> GetByCourseAsync(Guid courseId)
        {
            var all = await _repository.GetAll();
            return all.Where(r => r.CourseId == courseId).ToList();
        }

        public async Task<bool> AddAsync(Guid courseId, Guid prerequisiteCourseId)
        {
            // Evita duplicados e auto-referência
            if (courseId == prerequisiteCourseId) return false;

            var all = await _repository.GetAll();
            var exists = all.Any(r => r.CourseId == courseId && r.PrerequisiteCourseId == prerequisiteCourseId);
            if (exists) return false;

            var entity = new CourseRequirement
            {
                Id                   = Guid.NewGuid(),
                CourseId             = courseId,
                PrerequisiteCourseId = prerequisiteCourseId
            };

            await _repository.Add(entity);
            await _uow.Commit();
            return true;
        }

        public async Task RemoveAsync(Guid id)
        {
            await _repository.Remove(id);
            await _uow.Commit();
        }
    }
}
