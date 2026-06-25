using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("E_Payments");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.TransactionId)
                .HasColumnType("varchar(200)");

            builder.Property(e => e.Gateway)
                .HasColumnType("varchar(100)");

            builder.HasOne(e => e.User)
                .WithMany(e => e.Payments)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Order)
                .WithMany(e => e.Payments)
                .HasForeignKey(e => e.OrderId);
        }
    }
}
