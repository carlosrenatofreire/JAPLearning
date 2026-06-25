using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Relationships
{
    public class RolePermission : Entity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public Role Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
