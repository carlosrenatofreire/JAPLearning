using MundoDev.Business.Models.Enums;

namespace MundoDev.Business.Models.ViewModels.Entities
{
    public class PlanViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Months { get; set; }
        public decimal? Price { get; set; }
        public bool? Promotion { get; set; }
        public int? DiscountPercent { get; set; }
        public PlanType PlanType { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
    }
}
