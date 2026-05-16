namespace MundoDev.Business.Models.ViewModels.Entities
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Address { get; set; }
        public string? Number { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsActived { get; set; }
    }
}
