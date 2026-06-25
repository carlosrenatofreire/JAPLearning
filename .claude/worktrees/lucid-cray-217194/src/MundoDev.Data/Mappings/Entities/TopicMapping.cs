using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class TopicMapping : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("E_Topics");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(500)");

            builder.HasOne(e => e.Course)
                .WithMany(e => e.Topics)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
