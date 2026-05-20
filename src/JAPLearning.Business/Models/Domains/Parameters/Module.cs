using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Parameters
{
    public class Module : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
