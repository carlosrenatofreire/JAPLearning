namespace MundoDev.Business.Models.ViewModels.Entities
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Slug { get; set; }
        public string? CoverImage { get; set; }
        public string? Author { get; set; }
        public DateTime PublishDate { get; set; }
        public int? ReadingTime { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
    }
}
