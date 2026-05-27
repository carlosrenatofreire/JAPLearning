using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Data.Mappings.Parameters
{
    public class TeamMapping : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("P_Teams");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(1000)");

            builder.Property(e => e.Thumbnail)
                .HasColumnType("nvarchar(max)");

            builder.HasMany(e => e.Categories)
                .WithOne(e => e.Team)
                .HasForeignKey(e => e.TeamId);

            builder.HasMany(e => e.Users)
                .WithOne(e => e.Team)
                .HasForeignKey(e => e.TeamId);
        }
    }
}
