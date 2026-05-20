using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Data.Mappings.Parameters
{
    public class PermissionMapping : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("P_Permissions");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.Module)
                .WithMany(e => e.Permissions)
                .HasForeignKey(e => e.ModuleId);

            builder.HasMany(e => e.RolePermissions)
                .WithOne(e => e.Permission)
                .HasForeignKey(e => e.PermissionId);
        }
    }
}
