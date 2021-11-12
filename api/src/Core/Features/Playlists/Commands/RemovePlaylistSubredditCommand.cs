
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Playlists.Commands;
public class RemovePlaylistSubredditCommand : IRequest<Result>
{
    public Guid PlaylistId { get; set; }

    public Guid SubredditId { get; set; }
}
