using Futvibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Futvibe.Infrastructure.Persistence.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("ratings");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.RaterId).HasColumnName("rater_id");
        builder.Property(r => r.RatedId).HasColumnName("rated_id");
        builder.Property(r => r.MatchId).HasColumnName("match_id");
        builder.Property(r => r.Score).HasColumnName("score");
        builder.Property(r => r.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(r => new { r.RaterId, r.RatedId, r.MatchId }).IsUnique();
        builder.HasIndex(r => r.RatedId);
    }
}
