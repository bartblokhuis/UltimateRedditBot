
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Playlists.Commands;
public class AddSubredditToPlaylistCommand : IRequest<Result<PlaylistDto>>
{
    public Guid PlaylistId { get; set; }

    public Guid SubredditId { get; set; }
}
