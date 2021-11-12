using Core.Features.GuidChannels.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class GuildChannelsController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(Guid guildId, string discordChannelId)
    {
        return Ok(await Mediator.Send(new GetChannelByDiscordIdQuery(guildId, discordChannelId)));
    }
}
