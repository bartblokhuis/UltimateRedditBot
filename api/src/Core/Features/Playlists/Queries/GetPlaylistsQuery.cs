
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Playlists.Queries;
public class GetPlaylistsQuery : IRequest<ListResult<PlaylistDto>>
{
    public Guid GuildId { get; set; }

    public bool IgnoreGlobal { get; set; }

    public GetPlaylistsQuery(Guid guildId, bool ignoreGlobal)
    {
        GuildId = guildId;
        IgnoreGlobal = ignoreGlobal;
    }
}
