using JAPLearning.Business.Models.Domains.Relationships;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Entities
{
    public class Lesson : Entity
    {
        public Guid CourseId { get; set; }
        public Guid TopicId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TimeSpan? TimeLesson { get; set; }
        public string? Video { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsTest { get; set; }
        public bool IsFreePreview { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public Course Course { get; set; } = null!;
        public Topic Topic { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<UserCourseLesson> UserCourseLessons { get; set; } = new List<UserCourseLesson>();
        public ICollection<UserLessonTest> UserLessonTests { get; set; } = new List<UserLessonTest>();
    }
}
