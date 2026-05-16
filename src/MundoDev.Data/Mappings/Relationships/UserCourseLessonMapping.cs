using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Relationships;

namespace MundoDev.Data.Mappings.Relationships
{
    public class UserCourseLessonMapping : IEntityTypeConfiguration<UserCourseLesson>
    {
        public void Configure(EntityTypeBuilder<UserCourseLesson> builder)
        {
            builder.ToTable("UserCourseLessons");

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserCourseLessons)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Lesson)
                .WithMany(e => e.UserCourseLessons)
                .HasForeignKey(e => e.LessonId);
        }
    }
}
