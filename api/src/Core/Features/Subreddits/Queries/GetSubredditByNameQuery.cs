using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subreddits.Queries;

public class GetSubredditByNameQuery : IRequest<Result<SubredditDto>>
{
    public string Name { get; }

    public GetSubredditByNameQuery(string name)
    {
        Name = name;
    }
}

