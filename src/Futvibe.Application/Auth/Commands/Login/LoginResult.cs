using Futvibe.Application.Common.DTOs;

namespace Futvibe.Application.Auth.Commands.Login;

public record LoginResult(string Token, UserDto User);
