using Futvibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence;

public class FutvibeDbContext(DbContextOptions<FutvibeDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Banner> Banners => Set<Banner>();

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(FutvibeDbContext).Assembly);
}
