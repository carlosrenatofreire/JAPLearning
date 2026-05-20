using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Data.Mappings.Entities
{
    public class CertificateMapping : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.ToTable("E_Certificates");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CertifiedFile)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.ValidationCode)
                .HasColumnType("varchar(100)");

            builder.HasIndex(e => e.ValidationCode)
                .IsUnique();

            builder.HasOne(e => e.User)
                .WithMany(e => e.Certificates)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Course)
                .WithMany(e => e.Certificates)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
