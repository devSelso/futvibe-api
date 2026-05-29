using MediatR;

namespace Futvibe.Application.Auth.Commands.Refresh;

public record RefreshTokenCommand(string RawToken) : IRequest<RefreshResult>;
