using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Parameters
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
