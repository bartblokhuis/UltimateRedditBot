using Core.Features.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UserController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(string discordUserId)
    {
        return Ok(await Mediator.Send(new GetUserByDiscordId(discordUserId)));
    }
}
