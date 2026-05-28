using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Futvibe.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(u => u.Avatar).HasColumnName("avatar");
        builder.Property(u => u.Bio).HasColumnName("bio").HasMaxLength(200);

        builder.Property(u => u.Level)
            .HasColumnName("level")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(u => u.PresenceScore).HasColumnName("presence_score").HasDefaultValue(0);
        builder.Property(u => u.MatchesPlayed).HasColumnName("matches_played").HasDefaultValue(0);
        builder.Property(u => u.CreatedAt).HasColumnName("created_at").IsRequired();
    }
}
