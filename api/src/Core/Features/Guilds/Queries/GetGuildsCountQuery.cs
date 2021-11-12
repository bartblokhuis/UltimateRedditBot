using Domain.Wrapper;
using MediatR;

namespace Core.Features.Guilds.Queries;

public class GetGuildsCountQuery : IRequest<Result<int>>
{
}
