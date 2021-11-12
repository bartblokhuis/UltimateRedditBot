using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subreddits.Queries
{
    public class GetSubredditsQuery : IRequest<ListResult<SubredditDto>>
    {
    }
}
