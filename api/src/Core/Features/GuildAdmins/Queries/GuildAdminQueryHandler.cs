using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.GuildAdmins.Queries;

internal class GuildAdminQueryHandler : BaseCommandHandler, IRequestHandler<IsAdminQuery, Result<bool>>
{
    #region Constructor

    public GuildAdminQueryHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<Result<bool>> Handle(IsAdminQuery request, CancellationToken cancellationToken)
    {
        var isAdmin = await _context.GuildAdmins.AnyAsync(admin => admin.GuildId == request.GuildId && admin.UserId == request.UserId, cancellationToken);
        return await Result<bool>.SuccessAsync(isAdmin);
    }

    #endregion
}
