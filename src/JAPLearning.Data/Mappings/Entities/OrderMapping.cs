using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Data.Mappings.Entities
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("E_Orders");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Value)
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.Discount)
                .HasColumnType("decimal(10,2)");

            builder.HasOne(e => e.User)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Plan)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.PlanId);

            builder.HasOne(e => e.Status)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.StatusId);
        }
    }
}
