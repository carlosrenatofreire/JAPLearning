using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Data.Mappings.Relationships
{
    public class UserLessonQuestionMapping : IEntityTypeConfiguration<UserLessonQuestion>
    {
        public void Configure(EntityTypeBuilder<UserLessonQuestion> builder)
        {
            builder.ToTable("R_UserLessonQuestions");

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.UserLessonTest)
                .WithMany(e => e.UserLessonQuestions)
                .HasForeignKey(e => e.UserLessonTestId);

            builder.HasOne(e => e.Question)
                .WithMany(e => e.UserLessonQuestions)
                .HasForeignKey(e => e.QuestionId);

            builder.HasOne(e => e.SelectedOption)
                .WithMany(e => e.UserLessonQuestions)
                .HasForeignKey(e => e.SelectedOptionId);
        }
    }
}
