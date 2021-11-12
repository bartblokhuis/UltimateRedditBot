using Domain.Dtos;
using Domain.Enums;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Guilds.Commands;

public class UpdateGuildSettingsCommand : IRequest<Result<GuildDto>>
{
    #region Properties

    public Guid GuildId { get; set; }

    public string Prefix { get; set; }

    public bool AllowNsfw { get; set; }

    public bool IsEnabled { get; set; }

    public Sort Sort { get; set; }

    #endregion
}

