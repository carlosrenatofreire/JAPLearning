using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class LessonMapping : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Video)
                .HasColumnType("varchar(500)");

            builder.HasOne(e => e.Course)
                .WithMany(e => e.Lessons)
                .HasForeignKey(e => e.CourseId);

            builder.HasOne(e => e.Topic)
                .WithMany(e => e.Lessons)
                .HasForeignKey(e => e.TopicId);
        }
    }
}
