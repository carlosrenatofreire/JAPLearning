using JAPLearning.Business.Models.Enums;

namespace JAPLearning.Mvc.ViewModels.Entities
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string? TransactionId { get; set; }
        public string? Gateway { get; set; }
        public DateTime PaidDate { get; set; }
    }
}
