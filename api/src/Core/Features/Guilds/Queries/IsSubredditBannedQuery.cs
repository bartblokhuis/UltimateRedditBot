using Domain.Wrapper;
using MediatR;

namespace Core.Features.Guilds.Queries;

public class IsSubredditBannedQuery : IRequest<Result<bool>>
{
    public Guid GuildId { get; set; }

    public Guid SubredditId { get; set; }

    public IsSubredditBannedQuery(Guid guildId, Guid subredditId)
    {
        GuildId = guildId;
        SubredditId = subredditId;
    }
}

