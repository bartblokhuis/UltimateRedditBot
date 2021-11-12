using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Users.Commands;

internal class UserCommandHandler : BaseCommandHandler, IRequestHandler<AddUserCommand, Result<UserDto>>
{
    #region Constructor

    public UserCommandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<Result<UserDto>> Handle(AddUserCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrEmpty(command.DiscordUserId))
            throw new ArgumentNullException(nameof(command.DiscordUserId));

        var user = _mapper.Map<User>(command);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var mappedUser = _mapper.Map<UserDto>(user);
        return await Result<UserDto>.SuccessAsync(mappedUser);
    }

    #endregion
}

