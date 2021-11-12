using Domain.Wrapper;
using MediatR;

namespace Core.Features.BannedSubreddits.Commands;

public class UnBanSubredditCommand : IRequest<Result>
{
    #region Properties

    public Guid GuildId { get; set; }

    public Guid SubredditId { get; set; }

    #endregion
}
