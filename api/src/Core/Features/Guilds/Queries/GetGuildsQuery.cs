using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Guilds.Queries
{
    public class GetGuildsQuery : IRequest<ListResult<GuildDto>>
    {
    }
}
