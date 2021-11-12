using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.GuildAdmins.Commands;

internal class GuildAdminCommandHandler : BaseCommandHandler, IRequestHandler<AddGuildAdminCommand, IResult>, IRequestHandler<RemoveAdminCommand, IResult>
{
    #region Constructor

    public GuildAdminCommandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<IResult> Handle(AddGuildAdminCommand command, CancellationToken cancellationToken)
    {
        if(command == null)
            throw new ArgumentNullException(nameof(command));

        var admin = await _context.GuildAdmins.FirstOrDefaultAsync(admin => admin.GuildId == command.GuildId && admin.UserId == command.UserId, cancellationToken);
        if (admin != null)
            return await Result.FailAsync("User is already an admin");

        var guildAdmin = _mapper.Map<GuildAdmin>(command);

        await _context.GuildAdmins.AddAsync(guildAdmin, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<IResult> Handle(RemoveAdminCommand command, CancellationToken cancellationToken)
    {
        var admin = await _context.GuildAdmins.FirstOrDefaultAsync(admin => admin.GuildId == command.GuildId && admin.UserId == command.UserId, cancellationToken);
        if (admin == null)
            return Result.Fail("User is not a mod");

        _context.GuildAdmins.Remove(admin);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success("User is no longer an admin");
    }

    #endregion
}
