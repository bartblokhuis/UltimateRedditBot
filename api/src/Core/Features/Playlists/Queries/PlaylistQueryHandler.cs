
using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Playlists.Queries;
public class PlaylistQueryHandler : BaseQueryHandler, IRequestHandler<GetPlaylistByNameQuery, Result<PlaylistDto>>, IRequestHandler<GetPlaylistsQuery, ListResult<PlaylistDto>>
{
    #region Fields

    #endregion

    #region Constructor

    public PlaylistQueryHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<Result<PlaylistDto>> Handle(GetPlaylistByNameQuery request, CancellationToken cancellationToken)
    {
        var playlist = await _context.Playlists.Include(x => x.Subreddits).FirstOrDefaultAsync(playlist => (playlist.IsGlobal || playlist.GuildId == request.GuidId) && playlist.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (playlist == null)
            return (await Result.FailAsync("Playlist doesn't exist")) as Result<PlaylistDto>;

        var dto = _mapper.Map<PlaylistDto>(playlist);
        dto.Subreddits = _mapper.Map<IEnumerable<SubredditDto>>(playlist.Subreddits);
        return await Result<PlaylistDto>.SuccessAsync(dto);
        
    }
    
    public async Task<ListResult<PlaylistDto>> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Playlists.Include(x => x.Subreddits);
        ListResult<Playlist> playlists;
        if (!request.IgnoreGlobal)
            playlists = await query.Where(playlist => playlist.IsGlobal || playlist.GuildId == request.GuildId).ToListResultAsync();
        else
            playlists = await query.Where(playlist => playlist.GuildId == request.GuildId).ToListResultAsync();

        if (!playlists.Data.Any())
            return (await Result.FailAsync("No playlists found")) as ListResult<PlaylistDto>;
        
        var dtos = _mapper.Map<ListResult<PlaylistDto>>(playlists); 
        return dtos;
    }

    #endregion

}
