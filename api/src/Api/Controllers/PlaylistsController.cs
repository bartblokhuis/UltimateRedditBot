
using Core.Features.Playlists.Commands;
using Core.Features.Playlists.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers;
public class PlaylistsController : BaseController
{
    /*[HttpGet("Global")]
    public async Task<IActionResult> GetGlobalPlaylists()
    {

    }}*/

    [HttpGet("GetByGuildId")]
    public async Task<IActionResult> GetGuildPlaylists(Guid guildId, bool ignoreGlobal = false)
    {
        return Ok(await Mediator.Send(new GetPlaylistsQuery(guildId, ignoreGlobal)));
    }

    [HttpGet("GetByName")]
    public async Task<IActionResult> GetByName(string name, Guid guildId)
    {
        return Ok(await Mediator.Send(new GetPlaylistByNameQuery(name, guildId)));
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add(AddPlaylistCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("AddSubreddit")]
    public async Task<IActionResult> AddSubreddit(AddSubredditToPlaylistCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    
    [HttpDelete("Remove")]
    public async Task<IActionResult> Remove(RemovePlaylistCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    
    [HttpDelete("RemoveSubreddit")]
    public async Task<IActionResult> RemoveSubreddit(RemovePlaylistSubredditCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
