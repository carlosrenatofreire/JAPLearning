using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Parameters;

namespace MundoDev.Data.Mappings.Parameters
{
    public class LevelMapping : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.ToTable("Levels");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(255)");

            builder.HasMany(e => e.Courses)
                .WithOne(e => e.Level)
                .HasForeignKey(e => e.LevelId);
        }
    }
}
