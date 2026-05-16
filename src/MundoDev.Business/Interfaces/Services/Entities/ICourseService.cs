using MundoDev.Business.Interfaces.Services;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Interfaces.Services.Entities
{
    public interface ICourseService : IBaseService<Course>
    {
        Task<List<Course>> GetByCategoryAsync(Guid categoryId);
        Task<List<Course>> GetByTeacherAsync(Guid teacherId);
    }
}
