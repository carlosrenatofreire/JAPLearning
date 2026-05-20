using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Models.Domains.Relationships;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Entities
{
    public class Course : Entity
    {
        public Guid CategoryId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid LevelId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public int PassingScore { get; set; } = 60;
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsBrief { get; set; }
        public bool IsFree { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public Category Category { get; set; } = null!;
        public Teacher Teacher { get; set; } = null!;
        public Level Level { get; set; } = null!;
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<CourseRequirement> Requirements { get; set; } = new List<CourseRequirement>();
    }
}
