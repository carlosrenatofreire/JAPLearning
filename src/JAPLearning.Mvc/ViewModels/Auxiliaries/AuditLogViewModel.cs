using JAPLearning.Business.Models.Enums;

namespace JAPLearning.Mvc.ViewModels.Auxiliaries
{
    public class AuditLogViewModel
    {
        public Guid Id { get; set; }
        public LogType LogLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public string  CreatedBy      { get; set; } = string.Empty;
        public string? Action         { get; set; }
        public string? EntityName     { get; set; }
        public int?    HttpStatusCode { get; set; }
        public string? Message        { get; set; }
        public string? StackTrace     { get; set; }
        public string? Json           { get; set; }
    }
}
