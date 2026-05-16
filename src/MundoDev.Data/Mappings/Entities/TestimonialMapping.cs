using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class TestimonialMapping : IEntityTypeConfiguration<Testimonial>
    {
        public void Configure(EntityTypeBuilder<Testimonial> builder)
        {
            builder.ToTable("Testimonials");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.AuthorName)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Role)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Country)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.PhotoUrl)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.LinkedinUrl)
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Quote)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }
}
