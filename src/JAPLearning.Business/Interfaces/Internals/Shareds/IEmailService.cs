using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Interfaces.Internals.Shareds
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email emailData);
    }
}
