
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Playlists.Commands;
public class AddPlaylistCommand : IRequest<Result<PlaylistDto>>
{
    public string Name { get; set; }

    public Guid? GuildId { get; set; }

    public bool IsGlobal { get; set; }
}
