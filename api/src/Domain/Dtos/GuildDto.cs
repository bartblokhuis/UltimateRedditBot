namespace Domain.Dtos;

public class GuildDto
{
    #region Properties

    public Guid Id { get; set; }

    public string DiscordGuildId { get; set; }

    public GuildSettingDto Setting {  get; set; }

    #endregion
}