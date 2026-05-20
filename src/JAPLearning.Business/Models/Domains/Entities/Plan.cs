using JAPLearning.Business.Models.Enums;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Entities
{
    public class Plan : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Months { get; set; }
        public decimal? Price { get; set; }
        public bool? Promotion { get; set; }
        public int? DiscountPercent { get; set; }
        public PlanType PlanType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
