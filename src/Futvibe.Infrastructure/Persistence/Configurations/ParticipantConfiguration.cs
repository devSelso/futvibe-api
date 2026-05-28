using Futvibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Futvibe.Infrastructure.Persistence.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("participants");

        builder.HasKey(p => new { p.MatchId, p.UserId });
        builder.Property(p => p.MatchId).HasColumnName("match_id");
        builder.Property(p => p.UserId).HasColumnName("user_id");

        builder.Property(p => p.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.HasIndex(p => p.UserId);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
