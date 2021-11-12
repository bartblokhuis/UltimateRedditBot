using Core.Features.Guilds.Queries;
using Core.Features.Subreddits.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[AllowAnonymous]
public class StatisticsController : BaseController
{
    #region Methods

    
    [HttpGet("TotalGuildsCount")]
    public async Task<IActionResult> TotalGuildsCount()
    {
        return Ok(await Mediator.Send(new GetGuildsCountQuery()));
    }

    [HttpGet("TotalSubredditsCount")]
    public async Task<IActionResult> TotalSubredditsCount()
    {
        return Ok(await Mediator.Send(new GetSubredditsCountQuery()));
    }

    #endregion
}

