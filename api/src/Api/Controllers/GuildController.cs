using Core.Features.Guilds.Commands;
using Core.Features.Guilds.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class GuildController : BaseController
{
    [HttpGet("Get")]
    public async Task<IActionResult> Get(string discordGuildId)
    {
        return Ok(await Mediator.Send(new GetGuildByDiscordIdQuery(discordGuildId)));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetGuildsQuery()));
    }

    [HttpPut()]
    public async Task<IActionResult> Update(UpdateGuildSettingsCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("Leave")]
    public async Task<IActionResult> GuildLeave(GuildLeaveCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
