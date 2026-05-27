namespace JAPLearning.Mvc.ViewModels.Entities
{
    public class TopicViewModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public bool IsActived { get; set; }
    }
}
