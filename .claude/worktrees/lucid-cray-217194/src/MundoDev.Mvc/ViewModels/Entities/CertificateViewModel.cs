namespace MundoDev.Mvc.ViewModels.Entities
{
    public class CertificateViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string CertifiedFile { get; set; } = string.Empty;
        public string? ValidationCode { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}
