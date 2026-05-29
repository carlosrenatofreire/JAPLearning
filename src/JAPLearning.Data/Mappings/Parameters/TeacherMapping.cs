using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Data.Mappings.Parameters
{
    public class TeacherMapping : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("P_Teachers");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(max)");

            builder.Property(e => e.PhotoUrl)
                .HasColumnType("varchar(1000)");

            builder.HasMany(e => e.Courses)
                .WithOne(e => e.Teacher)
                .HasForeignKey(e => e.TeacherId);
        }
    }
}
