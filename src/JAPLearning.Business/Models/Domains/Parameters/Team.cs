using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Parameters
{
    public class Team : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
