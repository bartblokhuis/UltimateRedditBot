using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.GuidChannels.Commands;

public class AddChannelCommand : IRequest<Result<GuildChannelDto>>
{
    #region Properties

    public Guid GuildId { get; set; }

    public string DiscordChannelId { get; set; }

    #endregion
}
