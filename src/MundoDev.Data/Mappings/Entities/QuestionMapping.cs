using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class QuestionMapping : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(1000)");

            builder.HasOne(e => e.Lesson)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.LessonId);
        }
    }
}
