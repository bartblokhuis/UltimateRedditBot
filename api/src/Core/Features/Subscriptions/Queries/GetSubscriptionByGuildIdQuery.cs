
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subscriptions.Queries;
public class GetSubscriptionsByGuildIdQuery : IRequest<ListResult<SubscriptionDto>>
{
    public Guid GuildId { get; set; }

    public bool ShowNsfw { get; set; }

    public GetSubscriptionsByGuildIdQuery(Guid guildId, bool showNsfw)
    {
        GuildId = guildId;
        ShowNsfw = showNsfw;
    }
}
