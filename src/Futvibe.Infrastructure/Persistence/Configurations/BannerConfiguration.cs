using Futvibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Futvibe.Infrastructure.Persistence.Configurations;

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable("banners");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");

        builder.Property(b => b.ImageUrl).HasColumnName("image_url").IsRequired();
        builder.Property(b => b.Alt).HasColumnName("alt").HasMaxLength(100);
        builder.Property(b => b.Href).HasColumnName("href");
        builder.Property(b => b.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(b => b.DisplayOrder).HasColumnName("display_order").HasDefaultValue(0);
    }
}
