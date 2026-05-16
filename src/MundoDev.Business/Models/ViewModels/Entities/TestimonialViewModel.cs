namespace MundoDev.Business.Models.ViewModels.Entities
{
    public class TestimonialViewModel
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string? LinkedinUrl { get; set; }
        public string Quote { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int DisplayOrder { get; set; }
        public bool Featured { get; set; }
        public bool IsActived { get; set; }
    }
}
