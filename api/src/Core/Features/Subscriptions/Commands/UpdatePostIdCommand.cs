
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subscriptions.Commands;
public class UpdatePostIdCommand : IRequest<Result>
{
    public Guid SubscriptionId { get; set; }

    public string LastPostId { get; set; }
}
