using Futvibe.Application.Common.DTOs;

namespace Futvibe.Application.Auth;

public record LoginResult(string Token, string RawRefreshToken, UserDto User);
