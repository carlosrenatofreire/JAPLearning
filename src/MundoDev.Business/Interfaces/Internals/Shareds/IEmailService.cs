using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Interfaces.Internals.Shareds
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email emailData);
    }
}
