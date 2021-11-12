using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subreddits.Queries;

public class GetSubredditsCountQuery : IRequest<Result<int>>
{
}

