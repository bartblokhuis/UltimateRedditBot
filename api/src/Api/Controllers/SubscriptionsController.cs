using Core.Features.Subscriptions.Commands;
using Core.Features.Subscriptions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SubscriptionsController : BaseController
{
    #region Methods

    [HttpGet("All")]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        return Ok(await Mediator.Send(new GetSubsciptionsQuery()));
    }

    [HttpGet("GetByGuildId")]
    public async Task<IActionResult> GetByGuildId(Guid guildId, bool showNsfw = false)
    {
        return Ok(await Mediator.Send(new GetSubscriptionsByGuildIdQuery(guildId, showNsfw)));
    }

    [HttpPost("Subscribe")]
    public async Task<IActionResult> Subscribe(SubscribeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("Unsubscribe")]
    public async Task<IActionResult> Unsubscribe(UnSubscribeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("UpdatePostId")]
    public async Task<IActionResult> UpdatePostId(UpdatePostIdCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    #endregion
}
