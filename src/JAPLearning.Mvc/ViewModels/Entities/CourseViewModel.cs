namespace JAPLearning.Mvc.ViewModels.Entities
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public Guid LevelId { get; set; }
        public string LevelName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public string? SnapshotUrl { get; set; }
        public string? PdfFileUrl { get; set; }
        public int PassingScore { get; set; }
        public bool IsBrief { get; set; }
        public bool IsFree { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
    }
}
