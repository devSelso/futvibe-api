using MediatR;

namespace Futvibe.Application.Auth.Commands.Logout;

public record LogoutCommand(string RawToken) : IRequest;
