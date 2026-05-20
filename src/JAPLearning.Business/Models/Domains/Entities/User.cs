using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Models.Domains.Relationships;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Entities
{
    public class User : Entity
    {
        public Guid RoleId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Address { get; set; }
        public string? Number { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public Role Role { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<UserCourseLesson> UserCourseLessons { get; set; } = new List<UserCourseLesson>();
        public ICollection<UserLessonTest> UserLessonTests { get; set; } = new List<UserLessonTest>();
    }
}
