using Core.Features.PostHistories.Commands;
using Core.Features.PostHistories.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PostHistoriesController : BaseController
{
    #region Methods

    [HttpGet]
    public async Task<IActionResult> Get(Guid guildId, Guid subredditId, Sort sort)
    {
        return Ok(await Mediator.Send(new GetPostHistoryQuery(guildId, subredditId, sort)));
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddPostHistories command)
    {
        return Ok(await Mediator.Send(command));
    }

    #endregion
}
