namespace Futvibe.Application.Common.DTOs;

public record BannerDto(
    Guid Id,
    string ImageUrl,
    string? Alt,
    string? Href
);
