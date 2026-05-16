using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class CourseMapping : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(e => e.Subtitle)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(2000)");

            builder.Property(e => e.Thumbnail)
                .HasColumnType("varchar(500)");

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.CategoryId);

            builder.HasOne(e => e.Teacher)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.TeacherId);

            builder.HasOne(e => e.Level)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.LevelId);
        }
    }
}
