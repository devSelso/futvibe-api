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
    public string City { get; private set; } = default!;
    public MatchLevel Level { get; private set; }
    public int PresenceScore { get; private set; }
    public int MatchesPlayed { get; private set; }
    public int MatchesAttended { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public static User Create(string name, string email, string passwordHash, MatchLevel level, string city)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            City = city,
            Level = level,
            PresenceScore = 0,
            MatchesPlayed = 0,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateProfile(string name, string? bio, MatchLevel level, string city)
    {
        Name = name;
        Bio = bio;
        Level = level;
        City = city;
    }

    public void RecordMatchPresence(bool present)
    {
        MatchesPlayed++;
        if (present) MatchesAttended++;
        PresenceScore = MatchesPlayed == 0
            ? 0
            : (int)Math.Round((double)MatchesAttended / MatchesPlayed * 100.0);
    }
}
