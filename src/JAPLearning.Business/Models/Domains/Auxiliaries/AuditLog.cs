using JAPLearning.Business.Models.Enums;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Auxiliaries
{
    public class AuditLog : Entity
    {
        public LogType  LogLevel       { get; set; }
        public DateTime CreatedDate    { get; set; }
        public string   CreatedBy      { get; set; } = string.Empty;
        public string?  Action         { get; set; }   // Created | Updated | Deleted | Login | Http401 …
        public string?  EntityName     { get; set; }   // Course | User | Lesson …
        public int?     HttpStatusCode { get; set; }   // 401 | 403 | 404 | 500
        public string?  Message        { get; set; }
        public string?  StackTrace     { get; set; }
        public string?  Json           { get; set; }
    }
}
