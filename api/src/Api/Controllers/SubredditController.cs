using Core.Features.Subreddits.Commands;
using Core.Features.Subreddits.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SubredditController : BaseController
{
    [HttpGet("Get")]
    public async Task<IActionResult> Get(string subredditName)
    {
        return Ok(await Mediator.Send(new GetSubredditByNameQuery(subredditName)));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetSubredditsQuery()));
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add(AddSubredditCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
