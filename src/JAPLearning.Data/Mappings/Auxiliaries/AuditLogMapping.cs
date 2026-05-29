using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Data.Mappings.Auxiliaries
{
    public class AuditLogMapping : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("A_AuditLogs");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(e => e.Action)
                .HasColumnType("varchar(50)");

            builder.Property(e => e.EntityName)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Message)
                .HasColumnType("varchar(1000)");

            builder.Property(e => e.StackTrace)
                .HasColumnType("varchar(max)");

            builder.Property(e => e.Json)
                .HasColumnType("varchar(max)");
        }
    }
}
