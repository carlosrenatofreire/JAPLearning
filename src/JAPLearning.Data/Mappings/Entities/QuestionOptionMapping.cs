using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Data.Mappings.Entities
{
    public class QuestionOptionMapping : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.ToTable("E_QuestionOptions");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(1000)");

            builder.HasOne(e => e.Question)
                .WithMany(e => e.Options)
                .HasForeignKey(e => e.QuestionId);
        }
    }
}
