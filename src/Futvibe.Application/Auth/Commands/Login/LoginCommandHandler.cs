using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using Futvibe.Domain.Interfaces.Services;
using MediatR;

namespace Futvibe.Application.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepo,
    IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await userRepo.GetByEmailAsync(request.Email, ct)
            ?? throw new BusinessException("Credenciais inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BusinessException("Credenciais inválidas.");

        var token = jwtService.Generate(user);

        var dto = new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Avatar,
            user.Bio,
            user.Level.ToString().ToLower(),
            user.PresenceScore,
            user.MatchesPlayed);

        return new LoginResult(token, dto);
    }
}
