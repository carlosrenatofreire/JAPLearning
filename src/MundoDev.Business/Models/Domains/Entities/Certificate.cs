using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Entities
{
    public class Certificate : Entity
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public string CertifiedFile { get; set; } = string.Empty;
        public string? ValidationCode { get; set; }
        public DateTime CompletedDate { get; set; }

        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
