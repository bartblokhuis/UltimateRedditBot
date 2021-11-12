using Core.Features.GuildAdmins.Commands;
using Core.Features.GuildAdmins.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class GuildAdminsController : BaseController
{
    #region Methods

    [HttpGet]
    public async Task<IActionResult> IsAdmin(Guid userId, Guid guildId)
    {
        return Ok(await Mediator.Send(new IsAdminQuery(userId, guildId)));
    }

    [HttpPost("AddAdmin")]
    public async Task<IActionResult> AddAdmin(AddGuildAdminCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("RemoveAdmin")]
    public async Task<IActionResult> RemoveAdmin(RemoveAdminCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    #endregion
}
