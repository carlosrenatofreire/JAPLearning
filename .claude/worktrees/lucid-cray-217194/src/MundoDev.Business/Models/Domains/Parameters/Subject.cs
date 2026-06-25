using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Parameters
{
    public class Subject : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
