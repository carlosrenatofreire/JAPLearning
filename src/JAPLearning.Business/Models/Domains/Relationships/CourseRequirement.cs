using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Relationships
{
    public class CourseRequirement : Entity
    {
        public Guid CourseId { get; set; }
        public Guid PrerequisiteCourseId { get; set; }

        public Course Course { get; set; } = null!;
        public Course PrerequisiteCourse { get; set; } = null!;
    }
}
