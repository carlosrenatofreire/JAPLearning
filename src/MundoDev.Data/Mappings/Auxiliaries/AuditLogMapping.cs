using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Auxiliaries;

namespace MundoDev.Data.Mappings.Auxiliaries
{
    public class AuditLogMapping : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(e => e.Message)
                .HasColumnType("varchar(1000)");

            builder.Property(e => e.StackTrace)
                .HasColumnType("varchar(max)");

            builder.Property(e => e.Json)
                .HasColumnType("varchar(max)");
        }
    }
}
