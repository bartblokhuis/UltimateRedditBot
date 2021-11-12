using Domain.Enums;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.PostHistories.Queries;

public class GetPostHistoryQuery : IRequest<Result<PostHistoryDto>>
{
    #region Proerties

    public Guid GuildId { get; set; }

    public Guid SubredditId { get; set; }

    public Sort Sort { get; set; }

    #endregion

    #region Constructor

    public GetPostHistoryQuery(Guid guildId, Guid subredditId, Sort sort)
    {
        GuildId = guildId;
        SubredditId = subredditId;
        Sort = sort;
    }

    #endregion
}

