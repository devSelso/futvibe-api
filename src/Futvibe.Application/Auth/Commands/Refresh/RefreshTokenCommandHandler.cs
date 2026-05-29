using System.Security.Cryptography;
using System.Text;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using Futvibe.Domain.Interfaces.Services;
using MediatR;

namespace Futvibe.Application.Auth.Commands.Refresh;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepo,
    IUserRepository userRepo,
    IJwtService jwtService) : IRequestHandler<RefreshTokenCommand, RefreshResult>
{
    public async Task<RefreshResult> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.RawToken)));

        var existingToken = await refreshTokenRepo.GetByTokenHashAsync(hash, ct)
            ?? throw new BusinessException("Token inválido.");

        if (!existingToken.IsValid)
            throw new BusinessException("Token expirado ou revogado.");

        var user = await userRepo.GetByIdAsync(existingToken.UserId, ct)
            ?? throw new BusinessException("Usuário não encontrado.");

        existingToken.Revoke();

        var (rawNew, newRefreshToken) = RefreshTokenFactory.Create(user.Id);
        await refreshTokenRepo.AddAsync(newRefreshToken, ct);
        await refreshTokenRepo.SaveChangesAsync(ct);

        var accessToken = jwtService.Generate(user);

        return new RefreshResult(accessToken, rawNew);
    }
}
