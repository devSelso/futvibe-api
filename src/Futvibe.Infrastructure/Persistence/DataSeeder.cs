using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(FutvibeDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        var hash = BCrypt.Net.BCrypt.HashPassword("123456");

        var u1 = User.Create("Rafael Costa",  "teste@futvibe.app",   hash, MatchLevel.Advanced);
        var u2 = User.Create("Juliana Melo",  "juliana@futvibe.app", hash, MatchLevel.Intermediate);
        var u3 = User.Create("Bruno Alves",   "bruno@futvibe.app",   hash, MatchLevel.Beginner);
        var u4 = User.Create("Camila Torres", "camila@futvibe.app",  hash, MatchLevel.Advanced);
        var u5 = User.Create("Diego Nunes",   "diego@futvibe.app",   hash, MatchLevel.Intermediate);

        u1.UpdateProfile(u1.Name, "Jogando futevôlei há 8 anos. Apaixonado pelo esporte.", u1.Level);
        u2.UpdateProfile(u2.Name, "Competidora nos fins de semana. Sempre em busca de bons parceiros.", u2.Level);
        u3.UpdateProfile(u3.Name, "Novo no esporte, mas com muita vontade de aprender.", u3.Level);
        u4.UpdateProfile(u4.Name, "Ex-vôlei de praia. Migrei para o futevôlei e não me arrependo.", u4.Level);
        u5.UpdateProfile(u5.Name, "Jogo toda semana na Barra. Bora marcar!", u5.Level);

        u1.AddPresenceScore(98);  u1.SeedMatchesPlayed(142);
        u2.AddPresenceScore(91);  u2.SeedMatchesPlayed(67);
        u3.AddPresenceScore(75);  u3.SeedMatchesPlayed(12);
        u4.AddPresenceScore(96);  u4.SeedMatchesPlayed(89);
        u5.AddPresenceScore(84);  u5.SeedMatchesPlayed(45);

        db.Users.AddRange(u1, u2, u3, u4, u5);
        await db.SaveChangesAsync();

        var matches = new[]
        {
            CreateMatch("Pelada da Barra — Sábado",
                "Praia da Barra, Posto 9, Rio de Janeiro",
                new DateOnly(2026, 5, 30), new TimeOnly(8, 0),
                MatchLevel.Intermediate, 20, 6, MatchVisibility.Hybrid, u1.Id,
                new[] { (u2.Id, ParticipantStatus.Confirmed), (u3.Id, ParticipantStatus.Pending) }),

            CreateMatch("Treino Avançado Ipanema",
                "Praia de Ipanema, Posto 8, Rio de Janeiro",
                new DateOnly(2026, 5, 31), new TimeOnly(7, 0),
                MatchLevel.Advanced, 30, 4, MatchVisibility.Public, u4.Id,
                new[] { (u1.Id, ParticipantStatus.Confirmed) }),

            CreateMatch("Futevôlei Iniciantes — Domingo",
                "Praia de Copacabana, Posto 6, Rio de Janeiro",
                new DateOnly(2026, 6, 1), new TimeOnly(9, 0),
                MatchLevel.Beginner, 0, 8, MatchVisibility.Public, u3.Id,
                new[] { (u5.Id, ParticipantStatus.Confirmed), (u2.Id, ParticipantStatus.Confirmed) }),

            CreateMatch("Jogo Privado — Grupo Seleto",
                "Arena Beach, Barra da Tijuca, Rio de Janeiro",
                new DateOnly(2026, 5, 28), new TimeOnly(16, 0),
                MatchLevel.Advanced, 50, 4, MatchVisibility.Private, u4.Id,
                new[] { (u1.Id, ParticipantStatus.Confirmed), (u2.Id, ParticipantStatus.Confirmed), (u5.Id, ParticipantStatus.Confirmed) }),

            CreateMatch("Pelada do Meio de Semana",
                "Praia do Leblon, Posto 12, Rio de Janeiro",
                new DateOnly(2026, 6, 3), new TimeOnly(18, 0),
                MatchLevel.Intermediate, 15, 6, MatchVisibility.Hybrid, u5.Id,
                new[] { (u3.Id, ParticipantStatus.Pending) }),

            CreateMatch("Futevôlei Livre — Sábado Tarde",
                "Praia de São Conrado, Rio de Janeiro",
                new DateOnly(2026, 5, 30), new TimeOnly(14, 0),
                MatchLevel.Beginner, 0, 10, MatchVisibility.Public, u2.Id,
                new[] { (u3.Id, ParticipantStatus.Confirmed), (u5.Id, ParticipantStatus.Confirmed) }),

            CreateMatch("Desafio Inter-Bairros",
                "Praia do Flamengo, Rio de Janeiro",
                new DateOnly(2026, 6, 6), new TimeOnly(10, 0),
                MatchLevel.Intermediate, 25, 6, MatchVisibility.Public, u1.Id,
                new[] { (u4.Id, ParticipantStatus.Confirmed), (u5.Id, ParticipantStatus.Waitlist) }),
        };

        db.Matches.AddRange(matches);
        await db.SaveChangesAsync();

        var banners = new[]
        {
            Banner.Create("/banners/ifood.png",  "iFood",         "https://ifood.com.br", 1),
            Banner.Create("/banners/uber.png",   "Uber",          "https://uber.com",     2),
            Banner.Create("/banners/nubank.png", "Kirra Fitness", null,                   3),
        };

        db.Banners.AddRange(banners);
        await db.SaveChangesAsync();
    }

    private static Match CreateMatch(
        string title, string location,
        DateOnly date, TimeOnly time,
        MatchLevel level, decimal price, int maxPlayers,
        MatchVisibility visibility, Guid hostId,
        (Guid userId, ParticipantStatus status)[] extraParticipants)
    {
        var match = Match.Create(title, location, date, time, level, price, maxPlayers, visibility, hostId);
        foreach (var (userId, status) in extraParticipants)
            match.AddParticipant(userId, status);
        return match;
    }
}
