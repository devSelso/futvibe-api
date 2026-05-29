using Futvibe.Application.Auth;
using MediatR;

namespace Futvibe.Application.Auth.Commands.Register;

public record RegisterCommand(string Name, string Email, string Password, string Level) : IRequest<LoginResult>;
