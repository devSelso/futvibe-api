using Futvibe.Application.Banners.Queries.GetBanners;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Futvibe.WebApi.Controllers;

[ApiController]
[Route("api/banners")]
public class BannersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBanners(CancellationToken ct)
        => Ok(await mediator.Send(new GetBannersQuery(), ct));
}
