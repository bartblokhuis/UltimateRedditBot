using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Users.Commands;

public class AddUserCommand : IRequest<Result<UserDto>>
{
    #region Properties

    public string DiscordUserId { get; set; }

    #endregion
}

