using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Data.Mappings.Relationships
{
    public class CourseRequirementMapping : IEntityTypeConfiguration<CourseRequirement>
    {
        public void Configure(EntityTypeBuilder<CourseRequirement> builder)
        {
            builder.ToTable("R_CourseRequirements");

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Course)
                .WithMany(e => e.Requirements)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PrerequisiteCourse)
                .WithMany()
                .HasForeignKey(e => e.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
