using Futvibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Futvibe.Infrastructure.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("matches");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasColumnName("id");

        builder.Property(m => m.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        builder.Property(m => m.Location).HasColumnName("location").HasMaxLength(300).IsRequired();
        builder.Property(m => m.Date).HasColumnName("date").IsRequired();
        builder.Property(m => m.Time).HasColumnName("time").IsRequired();

        builder.Property(m => m.Level)
            .HasColumnName("level")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.PricePerPlayer)
            .HasColumnName("price_per_player")
            .HasColumnType("numeric(10,2)")
            .HasDefaultValue(0m);

        builder.Property(m => m.MaxPlayers).HasColumnName("max_players").IsRequired();

        builder.Property(m => m.Visibility)
            .HasColumnName("visibility")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.HostId).HasColumnName("host_id").IsRequired();
        builder.Property(m => m.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasIndex(m => m.Date);
        builder.HasIndex(m => m.Level);
        builder.HasIndex(m => m.HostId);

        // Map private backing field _participants
        builder.HasMany(m => m.Participants)
            .WithOne()
            .HasForeignKey(p => p.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(m => m.Participants).HasField("_participants");
    }
}
