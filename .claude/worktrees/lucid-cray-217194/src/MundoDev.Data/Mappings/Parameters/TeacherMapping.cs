using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Parameters;

namespace MundoDev.Data.Mappings.Parameters
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
                .HasColumnType("varchar(500)");

            builder.Property(e => e.PhotoUrl)
                .HasColumnType("varchar(500)");

            builder.HasMany(e => e.Courses)
                .WithOne(e => e.Teacher)
                .HasForeignKey(e => e.TeacherId);
        }
    }
}
