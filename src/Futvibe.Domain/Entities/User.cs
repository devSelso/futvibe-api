using Futvibe.Domain.Enums;

namespace Futvibe.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string? Avatar { get; private set; }
    public string? Bio { get; private set; }
    public MatchLevel Level { get; private set; }
    public int PresenceScore { get; private set; }
    public int MatchesPlayed { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public static User Create(string name, string email, string passwordHash, MatchLevel level)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            Level = level,
            PresenceScore = 0,
            MatchesPlayed = 0,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateProfile(string name, string? bio, MatchLevel level)
    {
        Name = name;
        Bio = bio;
        Level = level;
    }

    public void IncrementMatchesPlayed() => MatchesPlayed++;
    public void AddPresenceScore(int points) => PresenceScore += points;
    public void SeedMatchesPlayed(int count) => MatchesPlayed = count;
}
