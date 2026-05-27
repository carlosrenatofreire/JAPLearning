using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class CertificateService : BaseService<Certificate, ICertificateRepository>, ICertificateService
    {
        public CertificateService(IUnitOfWork uow, ICertificateRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public async Task<bool> HasCertificateAsync(Guid userId, Guid courseId)
        {
            var all = await _repository.GetAll();
            return all.Any(c => c.UserId == userId && c.CourseId == courseId);
        }

        public async Task<bool> IssueCertificateAsync(Guid userId, Guid courseId, int scorePercent)
        {
            if (await HasCertificateAsync(userId, courseId)) return true;

            var cert = new Certificate
            {
                UserId         = userId,
                CourseId       = courseId,
                ScorePercent   = scorePercent,
                CompletedDate  = DateTime.UtcNow,
                ValidationCode = Guid.NewGuid().ToString("N").ToUpper()[..12],
                CertifiedFile  = string.Empty
            };

            return await AddAsync(cert);
        }
    }
}
