using Futvibe.Application.Notifications.Commands.MarkNotificationRead;
using Futvibe.Application.Notifications.Queries.GetNotifications;
using Futvibe.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Futvibe.WebApi.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUnread(CancellationToken ct)
        => Ok(await mediator.Send(new GetNotificationsQuery(User.GetUserId()), ct));

    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(Guid id, CancellationToken ct)
    {
        await mediator.Send(new MarkNotificationReadCommand(id, User.GetUserId()), ct);
        return NoContent();
    }
}
