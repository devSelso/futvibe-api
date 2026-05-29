using Futvibe.Domain.Exceptions;

namespace Futvibe.Domain.Entities;

public class Rating
{
    public Guid Id { get; private set; }
    public Guid RaterId { get; private set; }
    public Guid RatedId { get; private set; }
    public Guid MatchId { get; private set; }
    public int Score { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Rating() { }

    public static Rating Create(Guid raterId, Guid ratedId, Guid matchId, int score)
    {
        if (raterId == ratedId)
            throw new BusinessException("Não é possível avaliar a si mesmo.");

        if (score < 0 || score > 5)
            throw new BusinessException("Nota deve ser entre 0 e 5.");

        return new Rating
        {
            Id = Guid.NewGuid(),
            RaterId = raterId,
            RatedId = ratedId,
            MatchId = matchId,
            Score = score,
            CreatedAt = DateTime.UtcNow
        };
    }
}
