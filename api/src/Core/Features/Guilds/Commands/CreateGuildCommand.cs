using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Guilds.Commands;

public class CreateGuildCommand : IRequest<Result<GuildDto>>
{
    public string DiscordGuildId { get; set; }
}