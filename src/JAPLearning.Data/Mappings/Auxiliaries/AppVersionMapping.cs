using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Data.Mappings.Auxiliaries
{
    public class AppVersionMapping : IEntityTypeConfiguration<AppVersion>
    {
        public void Configure(EntityTypeBuilder<AppVersion> builder)
        {
            builder.ToTable("E_AppVersions");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.VersionNumber)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(e => e.ReleaseDate)
                .IsRequired();

            builder.HasMany(e => e.Items)
                .WithOne(e => e.Version)
                .HasForeignKey(e => e.VersionId);
        }
    }
}
