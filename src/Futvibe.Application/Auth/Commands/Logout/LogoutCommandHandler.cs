using System.Security.Cryptography;
using System.Text;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Auth.Commands.Logout;

public class LogoutCommandHandler(
    IRefreshTokenRepository refreshTokenRepo) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken ct)
    {
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.RawToken)));
        var token = await refreshTokenRepo.GetByTokenHashAsync(hash, ct);

        if (token is not null && token.IsValid)
        {
            token.Revoke();
            await refreshTokenRepo.SaveChangesAsync(ct);
        }
    }
}
