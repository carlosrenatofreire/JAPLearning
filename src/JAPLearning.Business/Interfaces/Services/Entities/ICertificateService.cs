using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Services.Entities
{
    public interface ICertificateService : IBaseService<Certificate>
    {
        Task<bool> HasCertificateAsync(Guid userId, Guid courseId);
        Task<bool> IssueCertificateAsync(Guid userId, Guid courseId, int scorePercent);
    }
}
