using Domain.Wrapper;
using MediatR;

namespace Core.Features.GuildAdmins.Commands;

public class RemoveAdminCommand : IRequest<IResult>
{
    #region Properties

    public Guid UserId { get; set; }

    public Guid GuildId { get; set; }

    #endregion
}
