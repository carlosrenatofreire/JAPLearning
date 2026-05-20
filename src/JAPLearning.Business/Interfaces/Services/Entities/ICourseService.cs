using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Services.Entities
{
    public interface ICourseService : IBaseService<Course>
    {
        Task<List<Course>> GetByCategoryAsync(Guid categoryId);
        Task<List<Course>> GetByTeacherAsync(Guid teacherId);
    }
}
