using MundoDev.Business.Interfaces.Services;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Interfaces.Services.Entities
{
    public interface ITopicService : IBaseService<Topic>
    {
        Task<List<Topic>> GetByCourseAsync(Guid courseId);
    }
}
