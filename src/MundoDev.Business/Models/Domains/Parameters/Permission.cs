using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Parameters
{
    public class Permission : Entity
    {
        public Guid ModuleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public Module Module { get; set; } = null!;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
