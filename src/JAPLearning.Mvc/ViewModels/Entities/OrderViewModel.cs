namespace JAPLearning.Mvc.ViewModels.Entities
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public Guid PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartLicense { get; set; }
        public DateTime EndLicense { get; set; }
        public decimal Value { get; set; }
        public decimal Discount { get; set; }
        public Guid StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }
    }
}
