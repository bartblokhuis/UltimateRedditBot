using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Guilds.Queries;

public class GetGuildByDiscordIdQuery : IRequest<Result<GuildDto>>
{
    public string DiscordId { get; }

    public GetGuildByDiscordIdQuery(string discordId)
    {
        DiscordId = discordId;
    }
}
