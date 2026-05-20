using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Services.Entities
{
    public interface ITopicService : IBaseService<Topic>
    {
        Task<List<Topic>> GetByCourseAsync(Guid courseId);
    }
}
