
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Playlists.Commands;
public class RemovePlaylistCommand : IRequest<Result>
{
    public Guid PlaylistId { get; set; }
}
