using MundoDev.Business.Models.Enums;

namespace MundoDev.Mvc.ViewModels.Auxiliaries
{
    public class AuditLogViewModel
    {
        public Guid Id { get; set; }
        public LogType LogLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Json { get; set; }
    }
}
