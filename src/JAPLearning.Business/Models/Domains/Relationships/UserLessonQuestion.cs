using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Relationships
{
    public class UserLessonQuestion : Entity
    {
        public Guid UserLessonTestId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public bool? IsRight { get; set; }
        public DateTime? AnsweredDate { get; set; }

        public UserLessonTest UserLessonTest { get; set; } = null!;
        public Question Question { get; set; } = null!;
        public QuestionOption? SelectedOption { get; set; }
    }
}
