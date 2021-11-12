using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.GuidChannels.Queries;

public class GetChannelByDiscordIdQuery : IRequest<Result<GuildChannelDto>>
{
    #region Properties

    public Guid GuildId { get; set; }

    public string DiscordChannelId { get; set; }

    #endregion

    #region Constructors

    public GetChannelByDiscordIdQuery(Guid guildId, string discordChannelId)
    {
        GuildId = guildId;
        DiscordChannelId = discordChannelId;
    }

    #endregion
}

