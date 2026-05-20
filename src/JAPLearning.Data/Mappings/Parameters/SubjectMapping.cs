using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Data.Mappings.Parameters
{
    public class SubjectMapping : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("P_Subjects");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(255)");

            builder.HasMany(e => e.Articles)
                .WithOne(e => e.Subject)
                .HasForeignKey(e => e.SubjectId);
        }
    }
}
