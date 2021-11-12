using AutoMapper;
using Core.Features.Common;
using Core.Features.Users.Commands;
using Database;
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.Queries;

public class UserQueryHandler : BaseQueryHandler, IRequestHandler<GetUserByDiscordId, Result<UserDto>>
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public UserQueryHandler(UltimateRedditBotDbContext context, IMapper mapper, IMediator mediator) : base(context, mapper)
    {
        _mediator = mediator;
    }

    #endregion

    #region Methods

    public async Task<Result<UserDto>> Handle(GetUserByDiscordId request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.DiscordUserId == request.DiscordUserId, cancellationToken);
        if (user == null)
        {
            //If the user doesn't exist yet we register the user here.
           var addUserCommand = new AddUserCommand
           {
                DiscordUserId = request.DiscordUserId,
            };

            return await _mediator.Send(addUserCommand);
        }

        var mappedUser = _mapper.Map<UserDto>(user);
        return await Result<UserDto>.SuccessAsync(mappedUser);
    }

    #endregion

}

