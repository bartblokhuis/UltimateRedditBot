using Domain.Wrapper;
using MediatR;

namespace Core.Features.GuildAdmins.Commands;

public class AddGuildAdminCommand : IRequest<IResult>
{
    #region Properties

    public Guid UserId { get; set; }

    public Guid GuildId { get; set; }

    #endregion
}

