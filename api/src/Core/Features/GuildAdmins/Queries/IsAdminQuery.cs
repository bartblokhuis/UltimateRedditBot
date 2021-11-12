using Domain.Wrapper;
using MediatR;

namespace Core.Features.GuildAdmins.Queries;

public class IsAdminQuery : IRequest<Result<bool>>
{
    #region Properties

    public Guid UserId { get; set; }

    public Guid GuildId { get; set; }

    #endregion

    #region Constructor

    public IsAdminQuery(Guid userId, Guid guildId)
    {
        UserId = userId;
        GuildId = guildId;
    }

    #endregion
}
