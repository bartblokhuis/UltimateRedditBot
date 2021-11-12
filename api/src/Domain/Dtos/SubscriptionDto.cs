
using Domain.Enums;

namespace Domain.Dtos;

public class SubscriptionDto
{
    #region Properties

    public Guid Id { get; set; }

    public string LastPostId { get; set; } = "";

    public Sort Sort { get; set; }

    public string SubredditName { get; set; }

    public IEnumerable<ChannelSubscriptionDto> Channels { get; set; }

    #endregion

    #region Constructor

    public SubscriptionDto()
    {
        Channels = new List<ChannelSubscriptionDto>();
    }

    #endregion
}

public class ChannelSubscriptionDto
{
    public Guid Id {  get; set; }

    public Guid GuildId { get; set; }

    public string DiscordChannelId { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsShowNsfw { get; set; }
}
