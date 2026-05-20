using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Relationships
{
    public class UserCourseLesson : Entity
    {
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? WatchedSeconds { get; set; }

        public User User { get; set; } = null!;
        public Lesson Lesson { get; set; } = null!;
    }
}
