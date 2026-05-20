using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Relationships
{
    public class UserLessonTest : Entity
    {
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
        public int? Score { get; set; }
        public bool Passed { get; set; }
        public int AttemptNumber { get; set; } = 1;
        public DateTime CompletedDate { get; set; }

        public User User { get; set; } = null!;
        public Lesson Lesson { get; set; } = null!;
        public ICollection<UserLessonQuestion> UserLessonQuestions { get; set; } = new List<UserLessonQuestion>();
    }
}
