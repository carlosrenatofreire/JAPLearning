using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("E_Users");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.Property(e => e.Password)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.PhoneNumber)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.PhotoUrl)
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Address)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.Number)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.City)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.State)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Country)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.ResetToken)
                .HasColumnType("varchar(500)");

            builder.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);
        }
    }
}
