
using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Playlists.Commands;
public class PlaylistCommandHandler : BaseCommandHandler, IRequestHandler<AddPlaylistCommand, Result<PlaylistDto>>, IRequestHandler<AddSubredditToPlaylistCommand, Result<PlaylistDto>>, 
    IRequestHandler<RemovePlaylistCommand, Result>, IRequestHandler<RemovePlaylistSubredditCommand, Result>
{
    #region Constructor

    public PlaylistCommandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    #region Add

    public async Task<Result<PlaylistDto>> Handle(AddPlaylistCommand command, CancellationToken cancellationToken)
    {
        //Check if there is a global or guild playlist that has the same name.
        if (await _context.Playlists.AnyAsync(playlist => (playlist.IsGlobal || playlist.GuildId == command.GuildId) && playlist.Name.ToLower() == command.Name.ToLower(), cancellationToken))
            return await Result<PlaylistDto>.FailAsync("There already is a playlist with the same name");
        
        if (command.GuildId != null) 
        { 
            var guild = await _context.Guilds.Include(x => x.GuildSetting).Include(x => x.GuildPlaylists).FirstOrDefaultAsync(guild => guild.Id == command.GuildId, cancellationToken);
            if(guild == null)
                return await Result<PlaylistDto>.FailAsync("Guild not found");

            if(guild.GuildPlaylists.Count >= guild.GuildSetting.MaxPlaylists)
                return await Result<PlaylistDto>.FailAsync("Max playlists reached");
        }
            

        var playlist = _mapper.Map<Playlist>(command);
        await _context.Playlists.AddAsync(playlist, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PlaylistDto>(playlist);
        return (await Result<PlaylistDto>.SuccessAsync(dto, "Playlist created")) as Result<PlaylistDto>;
    }

    public async Task<Result<PlaylistDto>> Handle(AddSubredditToPlaylistCommand command, CancellationToken cancellationToken)
    {
        var playlist = await _context.Playlists.Include(x => x.Subreddits).FirstOrDefaultAsync(playlist => playlist.Id == command.PlaylistId);
        if (playlist == null)
            return (await Result<PlaylistDto>.FailAsync("Playlist not found")) as Result<PlaylistDto>;

        if (playlist.Subreddits.Any(subreddit => subreddit.Id == command.SubredditId))
            return (await Result<PlaylistDto>.FailAsync("Subreddit is already in the playlist")) as Result<PlaylistDto>;

        var subreddit = await _context.Subreddits.FirstOrDefaultAsync(subreddit => subreddit.Id == command.SubredditId);
        if (subreddit == null)
            return (await Result<PlaylistDto>.FailAsync("Subreddit not found")) as Result<PlaylistDto>;

        playlist.Subreddits.Add(subreddit);
        _context.Playlists.Update(playlist);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PlaylistDto>(playlist);
        dto.Subreddits = _mapper.Map<IEnumerable<SubredditDto>>(playlist.Subreddits);
        return (await Result<PlaylistDto>.SuccessAsync(dto, "Subreddit added")) as Result<PlaylistDto>;
    }

    #endregion

    #region Remove

    public async Task<Result> Handle(RemovePlaylistCommand command, CancellationToken cancellationToken)
    {
        var playlist = await _context.Playlists.Include(x => x.Subreddits).FirstOrDefaultAsync(playlist => playlist.Id == command.PlaylistId);
        if (playlist == null)
            return (await Result.FailAsync("Playlist not found")) as Result;

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync(cancellationToken);
        return (await Result.SuccessAsync("Playlist removed")) as Result;
    }

    public async Task<Result> Handle(RemovePlaylistSubredditCommand command, CancellationToken cancellationToken)
    {
        var playlist = await _context.Playlists.Include(x => x.Subreddits).FirstOrDefaultAsync(playlist => playlist.Id == command.PlaylistId);
        if (playlist == null)
            return (await Result.FailAsync("Playlist not found")) as Result;

        var subreddit = playlist.Subreddits.FirstOrDefault(sub => sub.Id == command.SubredditId);
        if(subreddit == null)
            return (await Result.FailAsync("Subreddit is not in the playlist")) as Result;

        playlist.Subreddits.Remove(subreddit);
        _context.Playlists.Update(playlist);
        await _context.SaveChangesAsync(cancellationToken);
        return (await Result.SuccessAsync("Subreddit removed from the playlist")) as Result;
    }

    #endregion

    #endregion
}
