using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Data.Mappings.Relationships
{
    public class UserLessonTestMapping : IEntityTypeConfiguration<UserLessonTest>
    {
        public void Configure(EntityTypeBuilder<UserLessonTest> builder)
        {
            builder.ToTable("R_UserLessonTests");

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserLessonTests)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Lesson)
                .WithMany(e => e.UserLessonTests)
                .HasForeignKey(e => e.LessonId);
        }
    }
}
