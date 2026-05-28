using Futvibe.Domain.Entities;

namespace Futvibe.Domain.Interfaces.Services;

public interface IJwtService
{
    string Generate(User user);
}
