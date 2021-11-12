namespace Domain.Dtos;

public class GuildChannelDto
{
    #region Properties

    public Guid Id { get; set; }

    public Guid GuildId { get; set; }

    public string DiscordChannelId { get; set; }

    #endregion
}
