using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Entities
{
    public class Question : Entity
    {
        public Guid LessonId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public Lesson Lesson { get; set; } = null!;
        public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
        public ICollection<UserLessonQuestion> UserLessonQuestions { get; set; } = new List<UserLessonQuestion>();
    }
}
