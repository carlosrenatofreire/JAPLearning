using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Entities
{
    public class Order : Entity
    {
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }
        public DateTime StartLicense { get; set; }
        public DateTime EndLicense { get; set; }
        public decimal Value { get; set; }
        public decimal Discount { get; set; }
        public Guid StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; }

        public User User { get; set; } = null!;
        public Plan Plan { get; set; } = null!;
        public OrderStatus Status { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
