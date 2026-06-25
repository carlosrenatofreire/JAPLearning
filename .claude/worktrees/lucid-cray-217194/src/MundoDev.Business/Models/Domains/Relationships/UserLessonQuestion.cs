using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Relationships
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
