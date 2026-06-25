using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Entities
{
    public class Article : Entity
    {
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Slug { get; set; }
        public string? CoverImage { get; set; }
        public string? Author { get; set; }
        public DateTime PublishDate { get; set; }
        public int? ReadingTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public Subject Subject { get; set; } = null!;
    }
}
