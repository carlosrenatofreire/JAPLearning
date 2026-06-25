using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Relationships
{
    public class CourseRequirement : Entity
    {
        public Guid CourseId { get; set; }
        public Guid PrerequisiteCourseId { get; set; }

        public Course Course { get; set; } = null!;
        public Course PrerequisiteCourse { get; set; } = null!;
    }
}
