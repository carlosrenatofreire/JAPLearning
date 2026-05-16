namespace MundoDev.Business.Models.ViewModels.Entities
{
    public class LessonViewModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public Guid TopicId { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TimeSpan? TimeLesson { get; set; }
        public string? Video { get; set; }
        public bool IsTest { get; set; }
        public bool IsFreePreview { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
    }
}
