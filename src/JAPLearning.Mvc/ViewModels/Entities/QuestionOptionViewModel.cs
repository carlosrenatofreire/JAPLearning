namespace JAPLearning.Mvc.ViewModels.Entities
{
    public class QuestionOptionViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string QuestionName { get; set; } = string.Empty;
        public string LessonName   { get; set; } = string.Empty;
        public string CourseName   { get; set; } = string.Empty;
        public string TeamName     { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsActived { get; set; }
    }
}
