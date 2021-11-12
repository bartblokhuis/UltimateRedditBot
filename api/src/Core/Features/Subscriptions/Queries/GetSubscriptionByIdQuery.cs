
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subscriptions.Queries;
public class GetSubscriptionByIdQuery : IRequest<Result<SubscriptionDto>>
{
    public Guid SubscriptionId { get; set; }

    public GetSubscriptionByIdQuery(Guid subscriptionId)
    {
        SubscriptionId = subscriptionId;
    }
}
