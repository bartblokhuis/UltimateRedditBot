using Core.Features.BannedSubreddits.Commands;
using Core.Features.Guilds.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BannedSubredditsController : BaseController
{
    #region Methods

    [HttpGet("IsBanned")]
    public async Task<IActionResult> IsBanned(Guid guildId, Guid subredditId)
    {
        return Ok(await Mediator.Send(new IsSubredditBannedQuery(guildId, subredditId)));
    }

    [HttpPost("Ban")]
    public async Task<IActionResult> BanSubreddit(BanSubredditCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("UnBan")]
    public async Task<IActionResult> UnBanSubreddit(UnBanSubredditCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    #endregion
}
