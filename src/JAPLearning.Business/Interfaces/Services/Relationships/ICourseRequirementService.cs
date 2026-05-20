using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Business.Interfaces.Services.Relationships
{
    public interface ICourseRequirementService
    {
        Task<List<CourseRequirement>> GetByCourseAsync(Guid courseId);
        Task<bool> AddAsync(Guid courseId, Guid prerequisiteCourseId);
        Task RemoveAsync(Guid id);
    }
}
