using MundoDev.Business.Models.Enums;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Models.Domains.Entities
{
    public class Payment : Entity
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string? TransactionId { get; set; }
        public string? Gateway { get; set; }
        public DateTime PaidDate { get; set; }

        public User User { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}
