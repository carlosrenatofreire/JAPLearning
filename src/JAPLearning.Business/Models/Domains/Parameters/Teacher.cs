using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Parameters
{
    public class Teacher : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
