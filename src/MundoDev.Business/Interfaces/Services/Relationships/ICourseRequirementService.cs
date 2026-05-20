using MundoDev.Business.Models.Domains.Relationships;

namespace MundoDev.Business.Interfaces.Services.Relationships
{
    public interface ICourseRequirementService
    {
        Task<List<CourseRequirement>> GetByCourseAsync(Guid courseId);
        Task<bool> AddAsync(Guid courseId, Guid prerequisiteCourseId);
        Task RemoveAsync(Guid id);
    }
}
