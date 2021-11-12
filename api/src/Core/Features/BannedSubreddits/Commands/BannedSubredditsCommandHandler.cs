using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.BannedSubreddits.Commands;

public class BannedSubredditsCommandHandler : BaseCommandHandler, IRequestHandler<UnBanSubredditCommand, Result>, IRequestHandler<BanSubredditCommand, Result>
{
    #region Constructor

    public BannedSubredditsCommandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<Result> Handle(UnBanSubredditCommand command, CancellationToken cancellationToken)
    {
        if(command == null)
            throw new ArgumentNullException(nameof(command));

        var bannedSubreddit = await _context.BannedSubreddits.FirstOrDefaultAsync(bannedSubreddit => bannedSubreddit.GuildId == command.GuildId && bannedSubreddit.SubredditId == command.SubredditId);
        if (bannedSubreddit == null)
            return new Result { Messages = new List<string>() { "Subreddit isn't banned" }, Succeeded = false };

        _context.BannedSubreddits.Remove(bannedSubreddit);
        await _context.SaveChangesAsync(cancellationToken);

        return new Result { Messages = new List<string>() { "Subreddit is unbanned" }, Succeeded = true };
    }

    public async Task<Result> Handle(BanSubredditCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new NullReferenceException(nameof(command));

        var guild = await _context.Guilds.FirstOrDefaultAsync(guild => guild.Id == command.GuildId, cancellationToken);
        if (guild == null)
            throw new NullReferenceException(nameof(guild));

        var subreddit = await _context.Subreddits.FirstOrDefaultAsync(subreddit => subreddit.Id == command.SubredditId, cancellationToken);
        if (subreddit == null)
            throw new NullReferenceException(nameof(subreddit));

        var bannedSubreddit = _mapper.Map<BannedSubreddit>(command);
        await _context.BannedSubreddits.AddAsync(bannedSubreddit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new Result() { Messages = new List<string> { "Subreddit banned" }, Succeeded = true };
    }

    #endregion
}

