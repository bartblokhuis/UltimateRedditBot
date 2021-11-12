using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Playlists.Queries;
public class GetPlaylistByNameQuery : IRequest<Result<PlaylistDto>>
{
    public string Name { get; set; }

    public Guid? GuidId { get; set; }

    public GetPlaylistByNameQuery(string name, Guid guidId)
    {
        Name = name;
        GuidId = guidId;
    }
}
