using System.Security.Cryptography;
using System.Text;
using Futvibe.Domain.Entities;

namespace Futvibe.Application.Auth;

internal static class RefreshTokenFactory
{
    internal static (string Raw, RefreshToken Entity) Create(Guid userId)
    {
        var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
        return (raw, RefreshToken.Create(userId, hash));
    }
}
