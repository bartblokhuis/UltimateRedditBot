
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subscriptions.Queries;
public class GetSubsciptionsQuery : IRequest<ListResult<SubscriptionDto>>
{

}
