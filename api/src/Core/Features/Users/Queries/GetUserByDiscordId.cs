using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Users.Queries;

public class GetUserByDiscordId : IRequest<Result<UserDto>>
{
    public string DiscordUserId { get; set; }

    public GetUserByDiscordId(string discordUserId)
    {
        DiscordUserId = discordUserId;
    }
}

