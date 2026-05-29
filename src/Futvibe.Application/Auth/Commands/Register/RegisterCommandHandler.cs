using Futvibe.Application.Auth;
using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using Futvibe.Domain.Interfaces.Services;
using MediatR;

namespace Futvibe.Application.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepo,
    IRefreshTokenRepository refreshTokenRepo,
    IJwtService jwtService) : IRequestHandler<RegisterCommand, LoginResult>
{
    public async Task<LoginResult> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existing = await userRepo.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new BusinessException("E-mail já cadastrado.");

        var level = Enum.Parse<MatchLevel>(request.Level, ignoreCase: true);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(request.Name, request.Email, passwordHash, level);

        await userRepo.AddAsync(user, ct);
        await userRepo.SaveChangesAsync(ct);

        var accessToken = jwtService.Generate(user);
        var (rawRefreshToken, refreshToken) = RefreshTokenFactory.Create(user.Id);

        await refreshTokenRepo.AddAsync(refreshToken, ct);
        await refreshTokenRepo.SaveChangesAsync(ct);

        var dto = new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Avatar,
            user.Bio,
            user.Level.ToString().ToLower(),
            user.PresenceScore,
            user.MatchesPlayed);

        return new LoginResult(accessToken, rawRefreshToken, dto);
    }
}
