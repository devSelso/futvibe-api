using Futvibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Futvibe.Infrastructure.Persistence.Configurations;

public class MatchActivityLogConfiguration : IEntityTypeConfiguration<MatchActivityLog>
{
    public void Configure(EntityTypeBuilder<MatchActivityLog> builder)
    {
        builder.ToTable("match_activity_logs");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).HasColumnName("id");
        builder.Property(l => l.MatchId).HasColumnName("match_id");
        builder.Property(l => l.UserId).HasColumnName("user_id");
        builder.Property(l => l.Action)
            .HasColumnName("action")
            .HasConversion<string>()
            .IsRequired();
        builder.Property(l => l.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(l => l.MatchId);

        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
