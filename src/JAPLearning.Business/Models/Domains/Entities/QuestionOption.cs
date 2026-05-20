using JAPLearning.Business.Models.Domains.Relationships;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Entities
{
    public class QuestionOption : Entity
    {
        public Guid QuestionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public Question Question { get; set; } = null!;
        public ICollection<UserLessonQuestion> UserLessonQuestions { get; set; } = new List<UserLessonQuestion>();
    }
}
