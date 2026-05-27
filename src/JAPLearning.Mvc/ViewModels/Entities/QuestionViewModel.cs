namespace JAPLearning.Mvc.ViewModels.Entities
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string TeamName   { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; }
        public ICollection<QuestionOptionViewModel> Options { get; set; } = new List<QuestionOptionViewModel>();
    }
}
