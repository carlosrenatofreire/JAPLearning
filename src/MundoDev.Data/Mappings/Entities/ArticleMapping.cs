using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Data.Mappings.Entities
{
    public class ArticleMapping : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Articles");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Content)
                .HasColumnType("varchar(max)");

            builder.Property(e => e.Slug)
                .HasColumnType("varchar(200)");

            builder.HasIndex(e => e.Slug)
                .IsUnique();

            builder.Property(e => e.CoverImage)
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Author)
                .HasColumnType("varchar(100)");

            builder.HasOne(e => e.Subject)
                .WithMany(e => e.Articles)
                .HasForeignKey(e => e.SubjectId);
        }
    }
}
