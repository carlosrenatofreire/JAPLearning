using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Domains.Relationships;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Parameters
{
    public class Role : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
