using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Data.Mappings.Auxiliaries
{
    public class AppVersionItemMapping : IEntityTypeConfiguration<AppVersionItem>
    {
        public void Configure(EntityTypeBuilder<AppVersionItem> builder)
        {
            builder.ToTable("E_AppVersionItems");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Order)
                .HasDefaultValue(0);
        }
    }
}
