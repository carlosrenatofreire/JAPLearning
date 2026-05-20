using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Data.Mappings.Relationships
{
    public class UserCourseLessonMapping : IEntityTypeConfiguration<UserCourseLesson>
    {
        public void Configure(EntityTypeBuilder<UserCourseLesson> builder)
        {
            builder.ToTable("R_UserCourseLessons");

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
