using Futvibe.Domain.Interfaces.Repositories;
using Futvibe.Domain.Interfaces.Services;
using Futvibe.Infrastructure.Auth;
using Futvibe.Infrastructure.Persistence;
using Futvibe.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Futvibe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FutvibeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IBannerRepository, BannerRepository>();

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
