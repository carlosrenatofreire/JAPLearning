using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Entities
{
    public class Testimonial : Entity
    {
        public Guid? UserId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string? LinkedinUrl { get; set; }
        public string Quote { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public int DisplayOrder { get; set; }
        public bool Featured { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public User? User { get; set; }
    }
}
