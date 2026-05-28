using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Banners.Queries.GetBanners;

public record GetBannersQuery : IRequest<IReadOnlyList<BannerDto>>;
