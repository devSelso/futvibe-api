using Futvibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Futvibe.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).HasColumnName("id");
        builder.Property(n => n.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(n => n.Type).HasColumnName("type").HasConversion<string>().IsRequired();
        builder.Property(n => n.MatchId).HasColumnName("match_id");
        builder.Property(n => n.Message).HasColumnName("message").HasMaxLength(500).IsRequired();
        builder.Property(n => n.IsRead).HasColumnName("is_read").HasDefaultValue(false).IsRequired();
        builder.Property(n => n.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasIndex(n => new { n.UserId, n.IsRead });
        builder.HasIndex(n => n.CreatedAt);
    }
}
