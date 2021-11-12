
using AutoMapper;
using Core.Features.Playlists.Commands;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;

namespace Core.Mappings;
public class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<AddPlaylistCommand, Playlist>();
        CreateMap<Playlist, PlaylistDto>();
        CreateMap<Subreddit, SubredditDto>();
        CreateMap<ListResult<Playlist>, ListResult<PlaylistDto>>();
    }
}
