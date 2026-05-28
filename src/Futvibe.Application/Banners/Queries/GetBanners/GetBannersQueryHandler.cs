using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Banners.Queries.GetBanners;

public class GetBannersQueryHandler(IBannerRepository bannerRepo)
    : IRequestHandler<GetBannersQuery, IReadOnlyList<BannerDto>>
{
    public async Task<IReadOnlyList<BannerDto>> Handle(GetBannersQuery request, CancellationToken ct)
    {
        var banners = await bannerRepo.GetActiveAsync(ct);
        return banners.Select(b => new BannerDto(b.Id, b.ImageUrl, b.Alt, b.Href)).ToList();
    }
}
